using System.Web;

namespace Frapid.Configuration
{
    public class DbConvention
    {
        public static string GetDbNameByConvention()
        {
            string url = HttpContext.Current.Request.Url.Authority;

            if (url.StartsWith("www."))
            {
                url = url.Replace("www.", "");
            }

            return url.Replace(".", "_");
        }

        public static bool IsValidDomain()
        {
            string url = GetDbNameByConvention();
            return DomainSerializer.Get().Contains(url);
        }

        public static string GetCatalog()
        {
            string url = GetDbNameByConvention();

            //By convention, the default database name is localhost
            return IsValidDomain() ? url : "localhost";
        }
    }
}