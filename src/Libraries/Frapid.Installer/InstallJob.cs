using System;
using System.Linq;
using Frapid.Configuration;
using Frapid.Installer.Helpers;
using Quartz;

namespace Frapid.Installer
{
    public class InstallJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string url = context.JobDetail.Key.Name;
            InstallerLog.Verbose($"Installing frapid on domain {url}.");

            try
            {
                var installer = new Tenant.Installer(url);
                installer.InstallAsync().Wait();

                var site = new ApprovedDomainSerializer().Get().FirstOrDefault(x => x.DomainName.Equals(url));
                DbInstalledDomains.AddAsync(site).Wait();
                new InstalledDomainSerializer().Add(site);
            }
            catch (Exception ex)
            {
                InstallerLog.Error("Could not install frapid on {url} due to errors. Exception: {Exception}", url, ex);
                throw;
            }
        }
    }
}