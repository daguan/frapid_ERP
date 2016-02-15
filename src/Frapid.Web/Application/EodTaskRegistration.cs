using System;
using Frapid.Web.Jobs;
using Quartz;
using Quartz.Impl;

namespace Frapid.Web
{
    public class EodTaskRegistration
    {
        public static void Register()
        {
            var factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<EndOfDayJob>().WithIdentity("DayEndTask", "Reminders").Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("DayEndTask", "Reminders")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 15)
                    .InTimeZone(TimeZoneInfo.Local))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}