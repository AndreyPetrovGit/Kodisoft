using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace Feeds.Services.Jobs
{
    public class FeedUpdateScheduler
    {
        private static IScheduler _scheduler;
        public static ISchedulerFactory _schedulerFactory;
        public static void Start()
        {
            var FeedDbUpdateJob = JobBuilder.Create<FeedDbUpdater>().Build();

            var FeedDbUpdateTrigger = TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule("* * * * * ?")
                .Build();

            _scheduler.ScheduleJob(FeedDbUpdateJob, FeedDbUpdateTrigger).Wait();

        }
        static FeedUpdateScheduler()
        {
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = _schedulerFactory.GetScheduler().Result;
            _scheduler.Start().Wait();
        }
    }
}
