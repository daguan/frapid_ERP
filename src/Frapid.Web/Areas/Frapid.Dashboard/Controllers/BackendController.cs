using System.Globalization;
using System.Web.Hosting;
using Frapid.Areas;

namespace Frapid.Dashboard.Controllers
{
    public class BackendController : FrapidController
    {
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