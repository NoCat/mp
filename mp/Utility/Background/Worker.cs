﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace mp.Utility
{
    public class Worker
    {
        List<Job> _jobs = new List<Job>();
        public List<Job> Jobs
        {
            get
            {
                return _jobs;
            }
        }

        public void Add(Job job)
        {
            job.Parent = this;
            Jobs.Add(job);
        }

        /// <summary>
        /// 非阻塞运行Worker
        /// </summary>
        public void StartAsync()
        {
            var thread = new Thread(new ThreadStart(Start));
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 运行Worker,阻塞当前线程
        /// </summary>
        public void Start()
        {
            while (true)
            {
                for (int i = 0; i < Jobs.Count; i++)
                {
                    var job = Jobs[i];
                    if (job.IsProcessing == false && job.LastExcute + job.Interval < DateTime.Now)
                    {
                        var t = new Thread(new ThreadStart(job.Excute));
                        t.IsBackground = true;
                        t.Start();
                    }
                }
                Thread.Sleep(200);
            }
        }

    }
}