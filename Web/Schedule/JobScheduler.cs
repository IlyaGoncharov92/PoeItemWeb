using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace Web.Schedule
{
    public class JobScheduler
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            
            var updateItemsJob = JobBuilder.Create<UpdateItemsJob>().Build();

            var updateItemsTrigger = TriggerBuilder
                    .Create()
                    .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second))
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(20).RepeatForever())
                    .Build();

            scheduler.ScheduleJob(updateItemsJob, updateItemsTrigger);
        }
    }
}