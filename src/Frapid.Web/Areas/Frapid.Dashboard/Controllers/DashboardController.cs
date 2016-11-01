using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Frapid.Dashboard.Controllers
{
    public class DashboardController : BackendController
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
    }
}