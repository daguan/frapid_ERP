using System.Globalization;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Framework;
using Serilog;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class WebsiteBuilderController : FrapidController
    {
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            if (this.Request?.Url != null)
            {
                this.CurrentDomain = this.Request.Url.DnsSafeHost;
                this.CurrentPageUrl = this.Request.Url.AbsoluteUri;
                this.Tenant = TenantConvention.GetTenant(this.CurrentDomain);
            }

            bool isStatic = TenantConvention.IsStaticDomain(this.CurrentDomain);

            if (isStatic)
            {
                //Static domains are strictly used for content caching only.
                context.Result = new HttpNotFoundResult("The requested page does not exist.");
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }

        public WebsiteBuilderController()
        {
            string theme = this.GetTheme();

            ViewBag.LayoutPath = GetLayoutPath(theme);
            ViewBag.Layout = this.GetLayout(theme);
            ViewBag.HomepageLayout = this.GetHomepageLayout(theme);

            Log.Verbose($"The layout path for \"{this.CurrentPageUrl}\" is \"{ViewBag.LayoutPath}\".");
            Log.Verbose($"The layout for \"{this.CurrentPageUrl}\" is \"{ViewBag.Layout}\".");
            Log.Verbose($"The homepage layout for \"{this.CurrentPageUrl}\" is \"{ViewBag.HomepageLayout}\".");
        }

        public string CurrentDomain { get; set; }
        public string Tenant { get; set; }
        public string CurrentPageUrl { get; set; }

        public static string GetLayoutPath(string theme = "")
        {
            string layout = Configuration.GetCurrentThemePath();

            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return null;
        }

        protected string GetTheme()
        {
            return Configuration.GetDefaultTheme();
        }

        protected string GetLayout(string theme = "")
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                theme = GetTheme();
            }

            return ThemeConfiguration.GetLayout(theme);
        }

        protected string GetHomepageLayout(string theme = "")
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                theme = GetTheme();
            }

            return ThemeConfiguration.GetHomepageLayout(theme);
        }

        protected string GetRazorView(string areaName, string path)
        {
            Log.Verbose($"Prepping Razor view for area \"{areaName}\" and view \"{path}\".");

            string tenant = TenantConvention.GetTenant();
            string theme = Configuration.GetDefaultTheme();

            Log.Verbose($"Resolved tenant \"{tenant}\" and theme \"{theme}\".");

            string overridePath = "~/Tenants/{0}/Areas/Frapid.WebsiteBuilder/Themes/{1}/Areas/{2}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, tenant, theme, areaName);

            Log.Verbose($"Checking if there is an overridden view present on the theme path \"{overridePath}\".");

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                Log.Verbose($"The view \"{path}\" was overridden by the theme \"{theme}\".");
                return overridePath;
            }

            overridePath = "~/Tenants/{0}/Areas/{1}/Themes/{2}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, tenant, areaName, theme);

            Log.Verbose($"Checking if there is an overridden view present on the tenant path \"{overridePath}\".");

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                Log.Verbose($"The view \"{path}\" was overridden by the tenant \"{tenant}\".");
                return overridePath;
            }

            string defaultPath = "~/Areas/{0}/Views/{1}";
            defaultPath = string.Format(CultureInfo.InvariantCulture, defaultPath, areaName, path);

            Log.Verbose($"The view \"{path}\" was located on area \"{areaName}\" on path \"{defaultPath}\".");

            return defaultPath;
        }

        protected string GetRazorView(string areaName, string controllerName, string actionName)
        {
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(areaName, path);
        }

        protected string GetRazorView<T>(string path) where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            return this.GetRazorView(registration.AreaName, path);
        }

        protected string GetRazorView<T>(string controllerName, string actionName)
            where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(registration.AreaName, path);
        }
    }
}