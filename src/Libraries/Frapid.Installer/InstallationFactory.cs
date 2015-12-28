using System;
using Frapid.Configuration;
using Quartz;
using Quartz.Impl;

namespace Frapid.Installer
{
    public static class InstallationFactory
    {
        public static void Setup(ApprovedDomain domain)
        {
            new DomainSerializer("DomainsInstalled.json").Add(domain);

            var factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<InstallJob>().WithIdentity(domain.DomainName, "install").Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(domain.DomainName, "install-trigger")
                .StartAt(DateTime.UtcNow.AddSeconds(5))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}