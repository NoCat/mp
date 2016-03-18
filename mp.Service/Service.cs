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
            //下载，启动后马上执行，不会自动退出
            CreateJob(typeof(DownloadJob));

            //更新pixivtag权重,每日0时1分执行
            CreateJob(typeof(UpdatePixivTagWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 1));

            //更新image权重,每日0时2分执行
            CreateJob(typeof(UpdateImageWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 2));

            //清理过期的临时文件,每日0时3分执行
            CreateJob(typeof(CleanExpiredFileJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 3));

            //采集,每日3时0分执行
            CreateJob(typeof(PickJob), CronScheduleBuilder.DailyAtHourAndMinute(3, 0));
            //CreateJob(typeof(PickJob));

            //获取每日top50相关信息
            CreateJob(typeof(GetPixivTopJob));

            // 生成sitemap,每日0时30分执行
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
