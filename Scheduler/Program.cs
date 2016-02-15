using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using System.Threading;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                scheduler.Start();

                //更新pixivtag权重
                CreateJob(typeof(UpdatePixivTagWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 2), scheduler);

                //更新image权重
                CreateJob(typeof(UpdateImageWeightJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 4), scheduler);

                // 生成sitemap
                CreateJob(typeof(GenerateSitemapJob), CronScheduleBuilder.DailyAtHourAndMinute(0, 30), scheduler);
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        static void CreateJob(Type jobType, IScheduleBuilder scheduleBuider, IScheduler scheduler)
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
