using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Quartz;
using System.IO;

namespace mp.Service
{
    class CleanExpiredFileJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var path = @"c:\mp\temp\";
            var directory = new DirectoryInfo(path);
            var now = DateTime.Now;
            foreach (FileInfo item in directory.GetFiles())
            {
                if ((now - item.CreationTime).TotalHours > 4)
                {
                    File.Delete(item.FullName);
                }
            }
        }
    }
}
