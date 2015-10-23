using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace mp.Utility
{
    public class CleanExpiredFileJob : Job
    {
        protected override void ExcuteCore(object param)
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/temp/");
            var directory = new DirectoryInfo(path);
            var now = DateTime.Now;
            foreach (FileInfo item in directory.GetFiles())
            {
                if ((now - item.CreationTime).Hours > 4)
                {
                    File.Delete(item.FullName);
                }
            }
        }
    }
}