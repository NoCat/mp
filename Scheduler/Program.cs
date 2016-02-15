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

                #region UpdatePixivTagWeight 更新pixiv标签权重
                IJobDetail UpdatePixivTagWeightJob = JobBuilder.Create<UpdatePixivTagWeightJob>()
                    .WithIdentity("UpdatePixivTagWeightJob", "group1")
                    .Build();
                ITrigger UpdatePixivTagWeightTrigger = TriggerBuilder.Create()
                    .WithIdentity("UpdatePixivTagWeightJobTrigger", "group1")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 1))
                    .Build();
                scheduler.ScheduleJob(UpdatePixivTagWeightJob, UpdatePixivTagWeightTrigger);
                #endregion

                #region GenerateSitemap 生成网站地图
                IJobDetail GenerateSitemapJob = JobBuilder.Create<GenerateSitemapJob>()
                    .WithIdentity("GenerateSitemapJob", "group1")
                    .Build();
                ITrigger GenerateSitemapTrigger = TriggerBuilder.Create()
                    .WithIdentity("GenerateSitemapTrigger", "group1")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 30))
                    .Build();
                scheduler.ScheduleJob(GenerateSitemapJob, GenerateSitemapTrigger);
                #endregion
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
