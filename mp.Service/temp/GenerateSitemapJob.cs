using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Quartz;
using Quartz.Impl;

using mp.DAL;
using mp.BLL;

namespace mp.Service
{
    class GenerateSitemapJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var manager = new ManagerCollection();

            var max = 50000;
            var page = 0;

            Directory.CreateDirectory(@"c:\mp\sitemap");

            while (true)
            {
                var list = manager.Images.Items.Where(i => i.State == ImageStates.Ready).OrderBy(i => i.ID).Select(i => i.ID).Skip(page * max).Take(max).ToList();
                using (var writer = System.IO.File.CreateText(string.Format(@"c:\mp\sitemap\sitemap_{0}.txt", page)))
                {
                    foreach (var item in list)
                    {
                        writer.WriteLine(string.Format("http://www.miaopass.net/image/{0}",item));
                    }
                }
                page++;
                if (list.Count < max)
                    break;
            }
        }
    }
}
