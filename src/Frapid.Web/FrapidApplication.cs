using System;
using System.Web;
using Frapid.Configuration;
using Serilog;

namespace Frapid.Web
{
    public sealed class FrapidApplication : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += this.App_BeginRequest;
            app.Error += this.App_Error;
        }

        private void App_Error(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var exception = context.Server.GetLastError();
            if (exception != null)
            {
                Log.Error("Exception. {exception}", exception);
            }
        }


        public void Dispose()
        {
        }

        private void App_BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return;
            }

            string domain = DbConvention.GetDomain();
            Log.Verbose($"Got a {context.Request.HttpMethod} request {context.Request.AppRelativeCurrentExecutionFilePath} on domain {domain}.");

            bool enforceSsl = DbConvention.EnforceSsl(domain);

            if (!enforceSsl)
            {
                Log.Verbose($"SSL was not enforced on domain {domain}.");
                return;
            }

            if (context.Request.Url.Scheme == "https")
            {
                context.Response.AddHeader("Strict-Transport-Security", "max-age=31536000");
            }
            else if (context.Request.Url.Scheme == "http")
            {
                string path = "https://" + context.Request.Url.Host +
                              context.Request.Url.PathAndQuery;
                context.Response.Status = "301 Moved Permanently";
                context.Response.AddHeader("Location", path);
            }
        }
    }
}