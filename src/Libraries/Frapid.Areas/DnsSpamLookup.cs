using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Hosting;
using Frapid.ApplicationState.CacheFactory;
using Frapid.Configuration;

namespace Frapid.Areas
{
    internal static class DnsSpamLookup
    {
        private static string[] GetRblServers()
        {
            string tenant = DbConvention.GetTenant();

            //Check RBL server list in tenant directory.
            string path = HostingEnvironment.MapPath($"/Tenants/{tenant}/Configs/RblServers.config");

            if (!File.Exists(path))
            {
                //Fallback to shared RBL server list.
                path = HostingEnvironment.MapPath($"/Resources/Configs/RblServers.config");
            }

            if (path == null || !File.Exists(path))
            {
                return new[] { "" };
            }

            string contents = File.ReadAllText(path, Encoding.UTF8);

            return contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool Query(string address)
        {
            var result = Dns.GetHostEntry(address);
            return result.AddressList.Any();
        }

        private static string ReverseIp(string ipAddress)
        {
            var segments = ipAddress.Split('.');
            return string.Join(".", segments.Reverse());
        }

        internal static DnsSpamLookupResult IsListedInSpamDatabase(string ipAddress)
        {
            bool isLoopBack = IPAddress.IsLoopback(IPAddress.Parse(ipAddress));

            if (isLoopBack)
            {
                return new DnsSpamLookupResult();
            }

            string key = ipAddress + ".spam.check";
            var factory = new DefaultCacheFactory();

            var isListed = factory.Get<DnsSpamLookupResult>(key);

            if (isListed == null)
            {
                isListed = FromStore(ipAddress);
                factory.Add(key, isListed, DateTimeOffset.UtcNow.AddHours(2));
            }

            return isListed;
        }

        internal static DnsSpamLookupResult FromStore(string ipAddress)
        {
            string prefix = ReverseIp(ipAddress);
            var lookupServers = GetRblServers();

            foreach (string server in lookupServers)
            {
                string address = prefix + "." + server;
                bool result = Query(address);

                if (result)
                {
                    return new DnsSpamLookupResult
                    {
                        IpAddress = ipAddress,
                        RblServer = server,
                        IsListed = true
                    };
                }
            }

            return new DnsSpamLookupResult();
        }
    }
}