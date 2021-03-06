﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using mp.BLL;

namespace mp.Utility
{
    public class PickJob : Job
    {
        string SelectorToXPath(string selector)
        {
            var result = "";
            //先按照空格分开
            var regex = new Regex(@"\S+");
            var matches = regex.Matches(selector);
            foreach (Match match in matches)
            {
                var node = "";
                //节点名，默认*
                var nodeName = "*";
                //筛选条件，默认无
                var predicate = "";
                //按照#，.（点号）分开
                var subRegex = new Regex(@"([#\.\w])([^#\.\s])+");
                var subMatches = subRegex.Matches(match.Value);
                foreach (Match sm in subMatches)
                {
                    var value = sm.Value;
                    if (value.StartsWith("#"))
                    {
                        predicate += string.Format("@id='{0}' and ", value.Substring(1));
                    }
                    else if (value.StartsWith("."))
                    {
                        predicate += string.Format("contains(@class,'{0}') and ", value.Substring(1));
                    }
                    else
                    {
                        nodeName = value;
                    }
                }
                node = nodeName;
                if (predicate != "")
                {
                    predicate = predicate.Substring(0, predicate.Length - 5);
                    node += string.Format("[{0}]", predicate);
                }
                result += "//" + node;
            }
            return result;
        }

        public PickJob()
        {
            Interval = TimeSpan.FromHours(3);
        }

        protected override void ExcuteCore(object param)
        {
            var manager = new ManagerCollection();
            var wc = new XWebClient();
            var now = DateTime.Now;
            var time = now.AddDays(-2);
            var list = manager.AdminPixivPickUsers.Items.Where(p => p.LastPickTime < time).ToList();
            if (list.Count == 0)
                return;

            //登录逻辑
            wc.Post("https://www.secure.pixiv.net/login.php", new { mode = "login", pixiv_id = "aabc_1123@qq.com", pass = "Horsea", skip = 1 });


            foreach (var user in list)
            {
                var isEnd = false;
                var pixivworkList = new List<PixivWork>();

                //查看作品列表
                var page = 1;
                while (true)
                {
                    var html = wc.Get("http://www.pixiv.net/member_illust.php?type=illust&id=" + user.PixivUserID + "&p=" + page);
                    //var html = wc.Get("http://www.pixiv.net/member_illust.php?type=illust&id=" + 163536 + "&p=" + page);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var workList = doc.DocumentNode.SelectNodes(SelectorToXPath(".work._work"));

                    foreach (var item in workList)
                    {
                        try
                        {
                            var work = new PixivWork();

                            var href = "http://www.pixiv.net" + item.Attributes["href"].Value;
                            work.From = href;

                            var workHtml = wc.Get(href);
                            var workDoc = new HtmlDocument();
                            workDoc.LoadHtml(workHtml);

                            var publishDateTimeNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".work-info .meta") + "/li[1]");
                            var publishDateTime = Convert.ToDateTime(publishDateTimeNode.InnerText);
                            if (publishDateTime < user.LastPickTime)
                            {
                                isEnd = true;
                                break;
                            }

                            foreach (var tagNode in workDoc.DocumentNode.SelectNodes(SelectorToXPath(".tag .text")))
                            {
                                work.Tags.Add(HttpUtility.HtmlDecode(tagNode.InnerText));
                            }

                            var sourceNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".original-image"));
                            var source = sourceNode.Attributes["data-src"].Value;
                            work.Source = source;

                            var usernameNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".user-link h1.user"));
                            var username = usernameNode.InnerText;
                            work.Username = username;

                            var titleNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".work-info .title"));
                            var title = titleNode.InnerText;
                            work.Title = title;

                            pixivworkList.Add(work);

                        }
                        catch { }
                    }

                    page++;
                    if (isEnd == true || workList.Count < 20)
                        break;
                }

                manager.Transaction(() =>
                {
                    foreach (var item in pixivworkList)
                    {
                        var mtextList = new List<string>();
                        item.Tags.ForEach(t =>
                        {
                            var tag = manager.AdminPixivTags.CreateIfNotExist(new AdminPixivTag { PText = t }, a => a.PText == t);
                            tag.CitationCount++;
                            if (string.IsNullOrWhiteSpace(tag.MText) == false)
                                mtextList.Add(tag.MText);
                            manager.AdminPixivTags.Update(tag);
                        });

                        var strTag = "";
                        foreach (var t in mtextList.Distinct())
                        {
                            strTag += string.Format("#{0}#", t);
                        }

                        manager.Picks.Add(item.From, item.Source, user.PackageID, string.Format("{0}[{1}]/[{2}]", strTag, item.Title, item.Username));
                    }

                    user.LastPickTime = now;
                    manager.AdminPixivPickUsers.Update(user);
                });
            }
        }

        class PixivWork
        {
            public string From { get; set; }
            public string Source { get; set; }
            public List<string> Tags { get; set; }
            public string Title { get; set; }
            public string Username { get; set; }

            public PixivWork()
            {
                Tags = new List<string>();
            }
        }
    }
}