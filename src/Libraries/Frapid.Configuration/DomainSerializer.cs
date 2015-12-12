using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Frapid.Configuration
{
    public static class DomainSerializer
    {
        public static List<string> Get()
        {
            List<string> urls = new List<string>();

            string path = HostingEnvironment.MapPath("~/Resources/Configs/domains.json");

            if (path == null)
            {
                return urls;
            }

            string contents = File.ReadAllText(path, Encoding.UTF8);
            urls = JsonConvert.DeserializeObject<List<string>>(contents);

            return urls;
        }

        public static void Save(List<string> urls)
        {
            string contents = JsonConvert.SerializeObject(urls);
            string path = HostingEnvironment.MapPath("~/Resources/Configs/domains.json");

            if (path == null)
            {
                return;
            }

            File.WriteAllText(path, contents, Encoding.UTF8);
        }
    }
}