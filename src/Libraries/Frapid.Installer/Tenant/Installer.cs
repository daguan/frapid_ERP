using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.Installer.Helpers;
using Frapid.Installer.Models;

namespace Frapid.Installer.Tenant
{
    public class Installer
    {
        public static List<Installable> InstalledApps;

        public Installer(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }

        public async Task InstallAsync()
        {
            InstalledApps = new List<Installable>();

            string tenant = TenantConvention.GetTenant(this.Url);
            InstallerLog.Verbose($"Creating database {tenant}.");
            var db = new DbInstaller(tenant);
            await db.InstallAsync().ConfigureAwait(false);

            InstallerLog.Verbose("Getting installables.");
            var installables = GetInstallables(tenant);

            foreach (var installable in installables)
            {
                try
                {
                    InstallerLog.Verbose($"Installing module {installable.ApplicationName}.");
                    await new AppInstaller(tenant, tenant, installable).InstallAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    InstallerLog.Error(ex.Message);
                    InstallerLog.Error($"Could not install module {installable.ApplicationName}.");
                }
            }
        }

        private static List<string> GetDefaultInstallableNames(string tenant)
        {
            string path = PathMapper.MapPath("~/Overrides/Configs/Applications.config");
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
            string root = PathMapper.MapPath("~/");
            var installables = new List<Installable>();

            if (root == null)
            {
                return installables;
            }

            var apps = AppResolver.Installables;

            foreach (var app in apps)
            {
                app.SetDependencies();

                if (app.AutoInstall &&
                    defaultApps.Contains(app.ApplicationName))
                {
                    installables.Add(app);
                }
            }

            return installables;
        }
    }
}