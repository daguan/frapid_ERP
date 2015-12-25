using System.Collections.Generic;
using System.IO;
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

            string contents = File.ReadAllText(path, Encoding.UTF8);
            domains = JsonConvert.DeserializeObject<List<ApprovedDomain>>(contents);

            return domains ?? new List<ApprovedDomain>();
        }

        public void Add(ApprovedDomain domain)
        {
            var domains = this.Get();
            domains.Add(domain);

            this.Save(domains);
        }

        public void Save(List<ApprovedDomain> urls)
        {
            string contents = JsonConvert.SerializeObject(urls);
            string path = HostingEnvironment.MapPath(Path + this.FileName);

            if (path == null)
            {
                return;
            }

            File.WriteAllText(path, contents, Encoding.UTF8);
        }
    }
}