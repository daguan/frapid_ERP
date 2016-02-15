using System;
using System.Configuration;
using System.Web;

namespace Frapid.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection)
            {
                return;
            }

            bool enforceSsl = ConfigurationManager.AppSettings["EnforceSSL"].ToLowerInvariant().Equals("true");

            if (!enforceSsl)
            {
                return;
            }

            if (this.Request.Url.Scheme == "https")
            {
                this.Response.AddHeader("Strict-Transport-Security", "max-age=31536000");
            }
            else if (this.Request.Url.Scheme == "http")
            {
                string path = "https://" + this.Request.Url.Host + this.Request.Url.PathAndQuery;
                this.Response.Status = "301 Moved Permanently";
                this.Response.AddHeader("Location", path);
            }
        }
    }
}