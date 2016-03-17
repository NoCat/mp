using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.BLL;
using mp.DAL;
using Newtonsoft.Json.Linq;

namespace mp.Service
{
    class GetPixivTop50Job : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var manager = new ManagerCollection();

                var wc = new XWebClient();
                var url = "http://www.pixiv.net/ranking.php?mode=daily&content=illust&p=1&format=json";

                var jsonStr = wc.Get(url);
                var json = JObject.Parse(jsonStr);

                var tagList = new List<AdminPixivTag>();
                var userList = new List<AdminPixivUser>();

                foreach (JObject item in json["contents"])
                {
                    var userId = (int)item["user_id"];
                    var userName = (string)item["user_name"];

                    AdminPixivUser user = null;
                    user = userList.Where(u => u.UserID == userId).FirstOrDefault();
                    if (user == null)
                    {
                        user = manager.AdminPixivUsers.CreateIfNotExist(
                           new AdminPixivUser
                           {
                               UserID = userId,
                               UserName = userName,
                               LastPickTime = new DateTime(1990, 1, 1),
                               State = AdminPixivUserStates.Wait
                           },
                           u => u.UserID == userId);
                        userList.Add(user);
                    }
                    user.RankCount++;

                    foreach (string item0 in item["tags"])
                    {
                        AdminPixivTag tag = null;

                        tag = tagList.Where(t => t.PText.Equals(item0, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                        if (tag == null)
                        {
                            tag = manager.AdminPixivTags.CreateIfNotExist(
                               new AdminPixivTag { PText = item0 },
                               t1 => t1.PText == item0);
                            tagList.Add(tag);
                        }
                        tag.Weight += 10000;
                    }
                }

                manager.AdminPixivUsers.UpdateRange(userList);
                manager.AdminPixivTags.UpdateRange(tagList);
            }
            catch { }
        }
    }
}
