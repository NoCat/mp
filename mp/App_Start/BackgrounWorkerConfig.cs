using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.Utility;

namespace mp
{
    public class BackgroundWorker
    {
        public void Start()
        {
            var worker = new Worker();
            var downloadJob = new DownloadJob { Interval = TimeSpan.Zero };
            worker.Add(downloadJob);
            worker.StartAsync();
        }
    }
}