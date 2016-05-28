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
    public class Installer
    {
        public Installer(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }

        public async Task InstallAsync()
        {
            var tenant = TenantConvention.GetTenant(this.Url);
            InstallerLog.Verbose($"Creating database {tenant}.");
            var db = new DbInstaller(tenant);
            await db.InstallAsync();

            InstallerLog.Verbose("Getting installables.");
            var installables = GetInstallables(tenant);
            InstallerLog.Information($"The following apps will be installed:\n\n {installables}.");

            foreach (var installable in installables)
            {
                InstallerLog.Verbose($"Installing module {installable.ApplicationName}.");
                await new AppInstaller(tenant, tenant, installable).InstallAsync();
            }
        }

        private static List<string> GetDefaultInstallableNames(string tenant)
        {
            var path = PathMapper.MapPath("~/Override/Configs/applications.json");
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
            var root = PathMapper.MapPath("~/");
            var installables = new List<Installable>();

            if (root == null)
            {
                return installables;
            }

            var files = Directory.GetFiles(root, "app_info.json", SearchOption.AllDirectories).ToList();

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