using HtmlAgilityPack;
using mp.BLL;
using mp.DAL;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace mp.Service
{
    class PickJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var manager = new ManagerCollection();
            var wc = new XWebClient();

            var list = manager.AdminPixivUsers.Items.Where(u=>u.State== AdminPixivUserStates.Pick).ToList();
            if (list.Count == 0)
                return;

            //登录逻辑
            var loginstr = wc.Post("https://www.pixiv.net/login.php", new { mode = "login", pixiv_id = "305870697@qq.com", pass = "Horsea", skip = 1 });

            foreach (var user in list)
            {
                var isEnd = false;
                var pixivworkList = new List<PixivWork>();

                //查看作品列表
                var page = 1;
                while (true)
                {
                    var html = wc.Get("http://www.pixiv.net/member_illust.php?type=illust&id=" + user.UserID + "&p=" + page);
                    //var html = wc.Get("http://www.pixiv.net/member_illust.php?type=illust&id=3402936");
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var workList = doc.DocumentNode.SelectNodes(SelectorToXPath(".work._work"));

                    if (workList == null)
                        break;

                    foreach (var item in workList)
                    {
                        try
                        {

                            var href = "http://www.pixiv.net" + item.Attributes["href"].Value;
                            //var href = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id=55670407";

                            var regex = new Regex(@"illust_id=(\d+)");
                            var match = regex.Match(href);

                            var workId = Convert.ToInt32(match.Groups[1].Value);

                            var workHtml = wc.Get(href);
                            var workDoc = new HtmlDocument();
                            workDoc.LoadHtml(workHtml);

                            var publishDateTimeNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".work-info .meta") + "/li[1]");
                            var publishDateTime = Convert.ToDateTime(publishDateTimeNode.InnerText);

                            if (publishDateTime <= user.LastPickTime)
                            {
                                isEnd = true;
                                break;
                            }

                            var tags = new List<string>();
                            foreach (var tagNode in workDoc.DocumentNode.SelectNodes(SelectorToXPath(".tag .text")))
                            {
                                tags.Add(HttpUtility.HtmlDecode(tagNode.InnerText));
                            }

                            var titleNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".work-info .title"));
                            var title = titleNode.InnerText;

                            var sourceNode = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath(".original-image"));

                            if (sourceNode != null)
                            {
                                var source = sourceNode.Attributes["data-src"].Value;
                                var work = new PixivWork();
                                work.From = href;
                                work.PublishDate = publishDateTime;
                                work.Tags.AddRange(tags);
                                work.Title = title;
                                work.Source = source;

                                pixivworkList.Add(work);
                                continue;
                            }

                            var multiple = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath("._work.multiple"));

                            if(multiple!=null)
                            {
                                var meta = workDoc.DocumentNode.SelectSingleNode(SelectorToXPath("ul.meta")).SelectSingleNode("li[2]");

                                var count = Convert.ToInt32( Regex.Match(meta.InnerText, @"(\d+)P").Groups[1].Value);
                                for (int i = 0; i < count; i++)
                                {
                                    var html3 = wc.Get(string.Format("http://www.pixiv.net/member_illust.php?mode=manga_big&illust_id={0}&page={1}",workId,i));
                                    var doc3 = new HtmlDocument();
                                    doc3.LoadHtml(html3);

                                    var img = doc3.DocumentNode.SelectSingleNode(SelectorToXPath("img"));
                                    var source = img.Attributes["src"].Value;

                                    var work = new PixivWork();
                                    work.From = href;
                                    work.PublishDate = publishDateTime;
                                    work.Tags.AddRange(tags);
                                    work.Title = title;
                                    work.Source = source;

                                    pixivworkList.Add(work);
                                }
                            }

                        }
                        catch { }
                    }

                    page++;
                    if (isEnd == true || workList.Count < 20)
                        break;
                }

                pixivworkList.Reverse();

                foreach (var item in pixivworkList)
                {
                    manager.Transaction(() =>
                    {
                        var work = new AdminPixivWork();
                        work.Description = item.Title;
                        work.UserName = user.UserName;
                        manager.AdminPixivWorks.Add(work);

                        var mtextList = new List<string>();
                        item.Tags.ForEach(t =>
                        {
                            var tag = manager.AdminPixivTags.CreateIfNotExist(new AdminPixivTag { PText = t }, a => a.PText == t);
                            if (string.IsNullOrWhiteSpace(tag.MText) == false)
                                mtextList.Add(tag.MText);

                            var workTag = new AdminPixivWorkTag();
                            workTag.WorkID = work.ID;
                            workTag.TagID = tag.ID;
                            manager.AdminPixivWorkTags.Add(workTag);
                        });

                        var strTag = "";
                        foreach (var t in mtextList.Distinct(StringComparer.OrdinalIgnoreCase))
                        {
                            strTag += string.Format("#{0}#", t);
                        }
                        if (string.IsNullOrWhiteSpace(user.UserName) == false)
                        {
                            strTag += string.Format("#{0}#", user.UserName);
                        }

                        var pick = manager.Picks.Add(item.From, item.Source, user.PackageID, string.Format("{0}{1}", strTag, work.Description));
                        work.ImageID = pick.ImageID;
                        manager.AdminPixivWorks.Update(work);

                        user.LastPickTime = item.PublishDate;
                        manager.AdminPixivUsers.Update(user);
                    });
                }
            }
        }

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

        class PixivWork
        {
            public int WorkID { get; set; }
            public string From { get; set; }
            public string Source { get; set; }
            public List<string> Tags { get; set; }
            public string Title { get; set; }
            public string Username { get; set; }
            public DateTime PublishDate { get; set; }

            public PixivWork()
            {
                Tags = new List<string>();
            }
        }

    }
}
