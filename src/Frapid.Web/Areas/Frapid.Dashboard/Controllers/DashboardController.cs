using System.Globalization;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;

namespace Frapid.Dashboard.Controllers
{
    public class DashboardController : FrapidController
    {
        private static readonly string LandingPage = "~/Areas/Frapid.Dashboard/Views/Default/LandingPage.cshtml";

        private string GetLayoutFile()
        {
            string theme = Configuration.GetDefaultTheme(this.Tenant);
            return ThemeConfiguration.GetLayout(this.Tenant, theme);
        }

        private string GetLayoutPath()
        {
            string layout = Configuration.GetCurrentThemePath(this.Tenant);
            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return null;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.ViewBag.LayoutPath = this.GetLayoutPath();
            this.ViewBag.LayoutFile = this.GetLayoutFile();

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                this.ViewBag.Layout = this.ViewBag.LayoutPath + this.ViewBag.LayoutFile;
            }
        }

        protected ContentResult FrapidView(string path, object model = null)
        {
            return this.View(this.HttpContext.Request.IsAjaxRequest() ? path : LandingPage, model);
        }

        protected string GetRazorView(string areaName, string controllerName, string actionName, string tenant)
        {
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(areaName, path, tenant);
        }

        protected string GetRazorView(string areaName, string path, string tenant)
        {
            string theme = Configuration.GetDefaultTheme(this.Tenant);

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


        protected string GetRazorView<T>(string path, string tenant) where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            return this.GetRazorView(registration.AreaName, path, tenant);
        }

        protected string GetRazorView<T>(string controllerName, string actionName, string tenant)
            where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(registration.AreaName, path, tenant);
        }
    }
}