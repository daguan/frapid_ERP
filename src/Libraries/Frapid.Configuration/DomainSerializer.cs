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

        public List<string> Get()
        {
            List<string> urls = new List<string>();

            string path = HostingEnvironment.MapPath(Path + this.FileName);

            if (path == null)
            {
                return urls;
            }

            string contents = File.ReadAllText(path, Encoding.UTF8);
            urls = JsonConvert.DeserializeObject<List<string>>(contents);

            return urls ?? new List<string>();
        }

        public void Add(string url)
        {
            var urls = this.Get();
            urls.Add(url);

            this.Save(urls);
        }

        public void Save(List<string> urls)
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