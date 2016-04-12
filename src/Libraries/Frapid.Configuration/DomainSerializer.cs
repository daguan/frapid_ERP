using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Frapid.Configuration
{
    public class DomainSerializer
    {
        public string FileName { get; set; }
        private const string Path = "~/Resources/Configs/";

        public DomainSerializer(string fileName)
        {
            this.FileName = fileName;
        }

        public List<ApprovedDomain> Get()
        {
            var domains = new List<ApprovedDomain>();

            string path = HostingEnvironment.MapPath(Path + this.FileName);

            if (path == null)
            {
                return domains;
            }

            if (!File.Exists(path))
            {
                return domains;
            }

            string contents = File.ReadAllText(path, Encoding.UTF8);
            domains = JsonConvert.DeserializeObject<List<ApprovedDomain>>(contents);

            return domains ?? new List<ApprovedDomain>();
        }

        public List<string> GetTenantMembers()
        {
            var domains = new List<string>();
            var approved = this.Get();

            foreach (var domain in approved)
            {
                domains.Add(domain.DomainName);
                domains.AddRange(domain.Synonyms);
                domains.Add(domain.CdnDomain);
            }

            return domains;
        }

        public void Add(ApprovedDomain domain)
        {
            var domains = this.Get();
            domains.Add(domain);

            this.Save(domains);
        }

        public void Save(List<ApprovedDomain> urls)
        {
            string contents = JsonConvert.SerializeObject(urls, Formatting.Indented);
            string path = HostingEnvironment.MapPath(Path + this.FileName);

            if (path == null)
            {
                return;
            }

            File.WriteAllText(path, contents, Encoding.UTF8);
        }
    }
}