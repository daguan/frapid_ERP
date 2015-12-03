using System.Globalization;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;

namespace Frapid.Dashboard.Controllers
{
    public class DashboardController : FrapidController
    {
        public DashboardController()
        {
            AppUsers.SetCurrentLogin();

            ViewBag.ViewPath = GetViewPath();
            ViewBag.LayoutPath = GetLayoutPath();
            ViewBag.LayoutFile = "Dashboard.cshtml";
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Layout = GetLayoutPath() + "Dashboard.cshtml";
            }
        }

        protected string GetViewPath()
        {
            return "/Areas/Frapid.Dashboard/Views";
        }

        protected string GetLayoutPath()
        {
            string layout = "~/Catalogs/{0}/Areas/Frapid.Dashboard/Views/Layouts/";
            string catalog = AppUsers.GetCatalog();
            layout = string.Format(CultureInfo.InvariantCulture, layout, catalog);

            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return "~/Areas/Frapid.Dashboard/Views/Layouts/";
        }
    }
}