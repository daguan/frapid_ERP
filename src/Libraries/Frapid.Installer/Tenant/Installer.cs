using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.Installer.Helpers;
using Frapid.Installer.Models;
using Newtonsoft.Json;

namespace Frapid.Installer.Tenant
{
    public static class AppResolver
    {
        public static List<Installable> Installables { get; set; }

        static AppResolver()
        {
            string root = PathMapper.MapPath("~/");
            var files = Directory.GetFiles(root, "AppInfo.json", SearchOption.AllDirectories).ToList();
            Installables = files.Select(file => File.ReadAllText(file, Encoding.UTF8)).Select(JsonConvert.DeserializeObject<Installable>).ToList();            
        }
    }

    public class Installer
    {
        public Installer(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }

        public async Task InstallAsync()
        {
            string tenant = TenantConvention.GetTenant(this.Url);
            InstallerLog.Verbose($"Creating database {tenant}.");
            var db = new DbInstaller(tenant);
            await db.InstallAsync().ConfigureAwait(false);

            InstallerLog.Verbose("Getting installables.");
            var installables = GetInstallables(tenant);
           
            foreach(var installable in installables)
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
            string path = PathMapper.MapPath("~/Override/Configs/Applications.config");
            var apps = ConfigurationManager.ReadConfigurationValue(path, "InstalledApplications").Or("").Split(',').Select(x => x.Trim()).ToList();

            return apps;
        }

        private static IEnumerable<Installable> GetInstallables(string tenant)
        {
            var defaultApps = GetDefaultInstallableNames(tenant);
            string root = PathMapper.MapPath("~/");
            var installables = new List<Installable>();

            if(root == null)
            {
                return installables;
            }

            var apps = AppResolver.Installables;

            foreach (var app in apps)
            {
                app.SetDependencies();

                if(app.AutoInstall &&
                   defaultApps.Contains(app.ApplicationName))
                {
                    installables.Add(app);
                }
            }

            return installables;
        }
    }
}