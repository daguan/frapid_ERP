using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.Installer.Models;
using Newtonsoft.Json;
using Quartz;

namespace Frapid.Installer
{
    public class InstallJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string url = context.JobDetail.Key.Name;
            string catalog = DbConvention.GetCatalog(url);
            var db = new DbInstaller(catalog);
            db.Install();

            var installables = GetInstallables();
            foreach (var installable in installables)
            {
                new AppInstaller(catalog, installable).Install();
            }
        }

        private static IEnumerable<Installable> GetInstallables()
        {
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

                if (app.AutoInstall)
                {
                    installables.Add(app);
                }
            }

            return installables;
        }
    }
}