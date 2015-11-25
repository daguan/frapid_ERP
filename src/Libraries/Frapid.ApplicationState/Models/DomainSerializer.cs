using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Frapid.ApplicationState.Models
{
    [Serializable]
    public static class DomainSerializer
    {
        public static List<string> Get()
        {
            List<string> urls = new List<string>();

            var path = HostingEnvironment.MapPath("~/Resources/Configs/domains.json");

            if (path != null)
            {
                var contents = File.ReadAllText(path, Encoding.UTF8);
                urls = JsonConvert.DeserializeObject<List<string>>(contents);
            }

            return urls;
        }

        public static void Save(List<string> urls)
        {
            var contents = JsonConvert.SerializeObject(urls);
            var path = HostingEnvironment.MapPath("~/Resources/Configs/domains.json");

            if (path != null)
            {
                File.WriteAllText(path, contents, Encoding.UTF8);
            }
        }
    }
}