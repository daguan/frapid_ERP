using System.Web.Mvc;

namespace Frapid.Dashboard.Controllers
{
    public class DefaultController : DashboardController
    {
        [Route("dashboard")]
        [Authorize]
        public ActionResult Index()
        {
            return View(GetRazorView<AreaRegistration>("Default/Index.cshtml"));
        }

        [Route("dashboard/apps")]
        [Authorize]
        public ActionResult GetApps()
        {
            return View(GetRazorView<AreaRegistration>("Default/Apps.cshtml"));
        }

        [Route("dashboard/foobar")]
        [Authorize]
        public ActionResult GetFooBar()
        {
            return View(GetRazorView<AreaRegistration>("Default/Foobar.cshtml"));
        }
    }
}