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
            worker.Add(new DownloadJob());
            worker.Add(new CleanExpiredFileJob());
            worker.StartAsync();
        }
    }
}