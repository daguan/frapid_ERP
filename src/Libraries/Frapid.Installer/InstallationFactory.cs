using System;
using Frapid.Configuration;
using Quartz;
using Quartz.Impl;

namespace Frapid.Installer
{
    public static class InstallationFactory
    {
        public static void Setup(string url)
        {
            new DomainSerializer("domains-installed.json").Add(url);

            var factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<InstallJob>().WithIdentity(url, "install").Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(url, "install-trigger")
                .StartAt(DateTime.UtcNow.AddSeconds(5))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}