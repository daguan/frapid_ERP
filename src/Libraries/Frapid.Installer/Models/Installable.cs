using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Frapid.Installer.Models
{
    public class Installable
    {
        public string ApplicationName { get; set; }
        public bool AutoInstall { get; set; }
        public string Thumbnail { get; set; }
        public string Publisher { get; set; }
        public string Url { get; set; }
        public string DocumentationUrl { get; set; }
        public string AssemblyName { get; set; }
        public string Version { get; set; }
        public DateTime? RealeasedOn { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Bundle { get; set; }
        public bool IsMeta { get; set; }
        public string DbSchema { get; set; }
        public string BlankDbPath { get; set; }
        public string SampleDbPath { get; set; }
        public bool InstallSample { get; set; }
        public string My { get; set; }
        public string OverrideTemplatePath { get; set; }
        public string OverrideDestination { get; set; }

        [JsonIgnore]
        public List<Installable> Dependencies { get; private set; }

        public List<string> DependsOn { get; set; }

        public void SetDependencies()
        {
            this.Dependencies = this.GetDependencies();
        }

        private List<Installable> GetDependencies()
        {
            var installables = new List<Installable>();

            if (this.DependsOn == null || this.DependsOn.Count().Equals(0))
            {
                return installables;
            }

            string root = HostingEnvironment.MapPath("~/");
            var files = new List<string>();

            if (root != null)
            {
                files = Directory.GetFiles(root, "AppInfo.json", SearchOption.AllDirectories).ToList();
            }

            foreach (var installable in files
                .Select(file => File.ReadAllText(file, Encoding.UTF8))
                .Select(JsonConvert.DeserializeObject<Installable>)
                .Where(installable => this.DependsOn.Contains(installable.ApplicationName)))
            {
                installable.SetDependencies();
                installables.Add(installable);
            }

            return installables;
        }
    }
}