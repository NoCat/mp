using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

using Quartz;
using Quartz.Impl;

namespace mp.Service
{
    class Service : ServiceControl
    {
        IScheduler scheduler;
        public bool Start(HostControl hostControl)
        {
            if (scheduler != null)
                return true;

            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //添加定时任务
            //更新pixivtag权重
            CreateJob(typeof(UpdatePixivTagWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 1));

            //更新image权重
            CreateJob(typeof(UpdateImageWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 2));

            //清理过期的临时文件
            CreateJob(typeof(CleanExpiredFileJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 3));

            //采集
            CreateJob(typeof(PickJob));

            // 生成sitemap
            CreateJob(typeof(GenerateSitemapJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 30));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (scheduler == null)
                return true;

            scheduler.Shutdown();
            scheduler = null;
            return true;
        }

        void CreateJob(Type jobType, IScheduleBuilder scheduleBuider=null)
        {
            IJobDetail job = JobBuilder.Create(jobType)
                .Build();

            var trigger = TriggerBuilder.Create()
                 .StartNow();

            if (scheduleBuider != null)
                trigger.WithSchedule(scheduleBuider);

            scheduler.ScheduleJob(job, trigger.Build());
        }
    }
}
