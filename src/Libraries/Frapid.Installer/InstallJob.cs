using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.Installer.Models;
using Newtonsoft.Json;
using Quartz;
using Serilog;

namespace Frapid.Installer
{
    public class InstallJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string url = context.JobDetail.Key.Name;

            try
            {
                Log.Verbose($"Installing frapid on domain {url}.");
                string tenant = DbConvention.GetTenant(url);

                Log.Verbose($"Creating database {tenant}.");
                var db = new DbInstaller(tenant);
                db.Install();

                Log.Verbose("Getting installables.");
                var installables = GetInstallables(tenant);
                Log.Information($"The following apps will be installed:\n\n {installables}.");

                foreach (var installable in installables)
                {
                    Log.Verbose($"Installing module {installable.ApplicationName}.");
                    new AppInstaller(tenant, tenant, installable).Install();
                }

                var site = new DomainSerializer("DomainsApproved.json").Get().FirstOrDefault(x => x.DomainName.Equals(url));
                DbInstalledDomains.Add(site);
                new DomainSerializer("DomainsInstalled.json").Add(site);
            }
            catch (Exception ex)
            {
                Log.Error("Could not install frapid on {url} due to errors. Exception: {Exception}", url, ex);
                throw;
            }
        }

        private static List<string> GetDefaultInstallableNames(string tenant)
        {
            string path = HostingEnvironment.MapPath("~/Override/Configs/Applications.config");
            var apps =
                ConfigurationManager.ReadConfigurationValue(path, "InstalledApplications")
                    .Or("")
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToList();

            return apps;
        }

        private static IEnumerable<Installable> GetInstallables(string tenant)
        {
            var defaultApps = GetDefaultInstallableNames(tenant);
            string root = HostingEnvironment.MapPath("~/");
            var installables = new List<Installable>();

            if (root == null)
            {
                return installables;
            }

            var files = Directory.GetFiles(root, "AppInfo.json", SearchOption.AllDirectories).ToList();

            foreach (var app in files
                .Select(file => File.ReadAllText(file, Encoding.UTF8))
                .Select(JsonConvert.DeserializeObject<Installable>))
            {
                app.SetDependencies();

                if (app.AutoInstall && defaultApps.Contains(app.ApplicationName))
                {
                    installables.Add(app);
                }
            }

            return installables;
        }
    }
}