using System.Globalization;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;

namespace Frapid.Dashboard.Controllers
{
    public class DashboardController : FrapidController
    {
        private static readonly string LandingPage = "~/Areas/Frapid.Dashboard/Views/Default/LandingPage.cshtml";

        public DashboardController()
        {
            ViewBag.LayoutPath = this.GetLayoutPath();
            ViewBag.LayoutFile = this.GetLayoutFile();
        }

        private string GetLayoutFile()
        {
            string theme = Configuration.GetDefaultTheme();
            return ThemeConfiguration.GetLayout(theme);
        }

        private string GetLayoutPath()
        {
            string layout = Configuration.GetCurrentThemePath();
            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return null;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Layout = GetLayoutPath() + this.GetLayoutFile();
            }
        }

        protected ContentResult FrapidView(string path, object model = null)
        {
            return this.View(this.HttpContext.Request.IsAjaxRequest() ? path : LandingPage, model);
        }

        protected string GetRazorView(string areaName, string path)
        {
            string tenant = DbConvention.GetTenant();
            string theme = Configuration.GetDefaultTheme();


            string overridePath = "~/Tenants/{0}/Areas/Frapid.Dashboard/Themes/{1}/Areas/{2}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, tenant, theme, areaName);

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                return overridePath;
            }


            overridePath = "~/Tenants/{0}/Areas/{1}/Themes/{2}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, tenant, areaName, theme);

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