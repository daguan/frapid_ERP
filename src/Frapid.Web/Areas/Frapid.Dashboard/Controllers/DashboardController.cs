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
        private static readonly string BasePath = "~/Areas/Frapid.Dashboard/Views";
        private static readonly string LandingPage = BasePath + "/Default/LandingPage.cshtml";
        private static readonly string LayoutByConvention = "~/Catalogs/{0}/Areas/Frapid.Dashboard/Views/Layouts/";
        private static readonly string FallbackLayout = "~/Areas/Frapid.Dashboard/Views/Layouts/";
        private static readonly string LayoutFile = "Dashboard.cshtml";

        public DashboardController()
        {
            AppUsers.SetCurrentLogin();

            ViewBag.ViewPath = GetViewPath();
            ViewBag.LayoutPath = GetLayoutPath();
            ViewBag.LayoutFile = LayoutFile;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.Layout = GetLayoutPath() + LayoutFile;
            }
        }

        protected ViewResultBase FrapidView(string path, object model = null)
        {
            return View(this.HttpContext.Request.IsAjaxRequest() ? path : LandingPage, model);
        }

        protected string GetViewPath(string view = "")
        {
            return BasePath + view;
        }

        protected string GetLayoutPath()
        {
            string layout = LayoutByConvention;
            string catalog = AppUsers.GetCatalog();
            layout = string.Format(CultureInfo.InvariantCulture, layout, catalog);

            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return FallbackLayout;
        }
    }
}