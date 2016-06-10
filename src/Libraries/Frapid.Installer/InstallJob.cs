using System;
using System.Linq;
using Frapid.Configuration;
using Frapid.Installer.Helpers;
using Quartz;

namespace Frapid.Installer
{
    public class InstallJob: IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            string url = context.JobDetail.Key.Name;
            InstallerLog.Verbose($"Installing frapid on domain {url}.");

            try
            {
                var installer = new Tenant.Installer(url);
                await installer.InstallAsync().ConfigureAwait(false);

                var site = new ApprovedDomainSerializer().Get().FirstOrDefault(x => x.DomainName.Equals(url));
                DbInstalledDomains.Add(site);
                new InstalledDomainSerializer().Add(site);
            }
            catch(Exception ex)
            {
                InstallerLog.Error("Could not install frapid on {url} due to errors. Exception: {Exception}", url, ex);
                throw;
            }
        }
    }
}