using System;
using System.Linq;
using System.Web;
using Frapid.Areas;
using Frapid.Configuration;
using Serilog;

namespace Frapid.Web
{
    public sealed class FrapidApplication : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += this.App_BeginRequest;
            app.EndRequest += this.App_EndRequest;
            app.PostAuthenticateRequest += this.App_PostAuthenticateRequest;

            app.Error += this.App_Error;
        }

        private void App_PostAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            string file = TenantStaticContentHelper.GetFile(HttpContext.Current);

            if (!string.IsNullOrWhiteSpace(file))
            {
                //We found the requested file on the tenant's "wwwroot" directory.
                HttpContext.Current.RewritePath(file);
            }
        }


        public void Dispose()
        {
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

        private void Handle404Error()
        {
            var context = HttpContext.Current;
            int statusCode = context.Response.StatusCode;

            if (statusCode != 404)
            {
                return;
            }

            context.Server.ClearError();
            context.Response.TrySkipIisCustomErrors = true;
            string path = context.Request.Url.AbsolutePath;

            var ignoredPaths = new[] { "/api", "/dashboard", "/content-not-found" };

            if (!ignoredPaths.Any(x => path.StartsWith(x)))
            {
                context.Server.TransferRequest("/content-not-found?path=" + path, true);
            }
        }

        public void App_EndRequest(object sender, EventArgs e)
        {
            this.Handle404Error();
        }

        public void App_BeginRequest(object sender, EventArgs e)
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