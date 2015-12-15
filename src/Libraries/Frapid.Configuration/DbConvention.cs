using System;
using System.Linq;
using System.Runtime.CompilerServices;
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

            var serializer = new DomainSerializer("domains-approved.json");

            return serializer.Get().Select(GetDbNameByConvention).Any(c => catalog.Equals(c));
        }

        public static string GetCatalog(string url = "")
        {
            string catalog = GetDbNameByConvention(url);
            //The default database name is localhost
            return IsValidCatalog(catalog) ? catalog : "localhost";
        }
    }
}