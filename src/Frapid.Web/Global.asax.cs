using System;
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

            string path = "https://" + this.Request.Url.Host + this.Request.Url.PathAndQuery;
            this.Response.Redirect(path);
        }
    }
}