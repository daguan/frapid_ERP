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

        public static string GetDbNameByConvention(string domain = "")
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = GetDomain();
            }

            return domain.Replace(".", "_");
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