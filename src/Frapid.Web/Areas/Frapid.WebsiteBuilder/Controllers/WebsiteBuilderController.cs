using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.Areas;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class WebsiteBuilderController : FrapidController
    {
        public WebsiteBuilderController()
        {
            string theme = GetTheme();

            ViewBag.LayoutPath = GetLayoutPath(theme);
            ViewBag.Layout = this.GetLayout(theme);
            ViewBag.HomepageLayout = this.GetHomepageLayout(theme);
        }

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
            string catalog = DbConvention.GetCatalog();
            string theme = Configuration.GetDefaultTheme();

            string overridePath = "~/Catalogs/{0}/Areas/Frapid.WebsiteBuilder/Themes/{1}/Areas/{2}/Views" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, catalog, theme, areaName);

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                return overridePath;
            }

            overridePath = "~/Catalogs/{0}/Areas/{1}/Themes/{2}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, catalog, areaName, theme);

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                return overridePath;
            }

            string defaultPath = "~/Areas/{0}/Views/{1}";
            defaultPath = string.Format(CultureInfo.InvariantCulture, defaultPath, areaName, path);

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