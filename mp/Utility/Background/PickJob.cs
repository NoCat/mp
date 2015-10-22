using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.Utility
{
    public class PickJob : Job
    {
        protected override void ExcuteCore(object param)
        {
            var db = new MiaopassContext();
            var wc = new XWebClient();
            var list = db.AdminPixivPickUsers.Where(p => p.LastPickDate < DateTime.Now.AddDays(-2)).ToList();
            if (list.Count == 0)
                return;

            try
            {
                //登录逻辑
                //
                //
                //
                //查看左偏列表
                var page = 1;
                while (true)
                {
                    var pageHtml = wc.Get("");

                }
            }
            catch { }
        }
    }
}