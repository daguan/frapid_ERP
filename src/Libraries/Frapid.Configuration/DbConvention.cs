using System.Linq;
using System.Web;

namespace Frapid.Configuration
{
    public class DbConvention
    {
        public static string GetDomain()
        {
            string url = HttpContext.Current.Request.Url.Authority;

            if (url.StartsWith("www."))
            {
                url = url.Replace("www.", "");
            }

            return url;
        }

        public static bool IsStaticDomain(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            var approved = new DomainSerializer("DomainsApproved.json");
            var approvedDomains = approved.Get();

            var tenant = approvedDomains.FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            return tenant != null && domain.StartsWith(tenant.CdnPrefix + ".");
        }

        private static string ConvertToDbName(string domain)
        {
            return domain.Replace(".", "_");
        }

        public static string GetDbNameByConvention(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            var approved = new DomainSerializer("DomainsApproved.json");
            var tenant = approved.Get().FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (tenant != null)
            {
                return ConvertToDbName(tenant.DomainName);
            }

            return ConvertToDbName(domain);
        }

        public static bool IsValidCatalog(string catalog = "")
        {
            if (string.IsNullOrWhiteSpace(catalog))
            {
                catalog = GetDbNameByConvention(catalog);
            }

            var serializer = new DomainSerializer("DomainsApproved.json");


            return serializer.Get().Any(domain => GetDbNameByConvention(domain.DomainName) == catalog);
        }

        public static string GetCatalog(string url = "")
        {
            string catalog = GetDbNameByConvention(url);
            //The default database name is localhost
            return IsValidCatalog(catalog) ? catalog : "localhost";
        }
    }
}