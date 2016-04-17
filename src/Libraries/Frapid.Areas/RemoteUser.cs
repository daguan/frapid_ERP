using System.Web;
using Frapid.i18n;

namespace Frapid.Areas
{
    public class RemoteUser
    {
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string Culture { get; set; }

        /// <summary>
        ///     Warning: Only works if your site is hosted in Cloudflare.
        /// </summary>
        public string Country { get; set; }


        public static bool IsListedInSpamDatabase()
        {
            var user = Get();
            string ip = user.IpAddress;

            if (string.IsNullOrWhiteSpace(ip))
            {
                return false;
            }

            var result = DnsSpamLookup.IsListedInSpamDatabase(ip);
            return result.IsListed;
        }

        public static RemoteUser Get(HttpContextBase context = null)
        {
            if (context == null)
            {
                context = new HttpContextWrapper(HttpContext.Current);
            }

            return new RemoteUser
            {
                Browser = context.Request.Browser.Browser,
                IpAddress = context.Request.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name,
                UserAgent = context.Request.UserAgent,
                Country = context.Request.ServerVariables["HTTP_CF_IPCOUNTRY"]
            };
        }
    }
}