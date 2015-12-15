using System.Globalization;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Configuration;
using Frapid.i18n;

namespace Frapid.Areas
{
    public abstract class FrapidController : Controller
    {
        public RemoteUser RemoteUser { get; private set; }

        protected FrapidController()
        {
            this.RemoteUser = new RemoteUser
            {
                Browser = this.Request?.Browser.Browser,
                IpAddress = this.Request?.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name
            };
        }


        protected string GetRazorView(string areaName, string path)
        {
            string catalog = DbConvention.GetCatalog();

            string overridePath = "~/Catalogs/{0}/Areas/{1}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, catalog, areaName);

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