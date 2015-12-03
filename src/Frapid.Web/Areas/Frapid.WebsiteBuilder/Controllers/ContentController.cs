using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class ContentController:DashboardController
    {
        [Route("dashboard/wb/contents")]
        [Authorize]
        public ActionResult Index(string alias = "")
        {
            return View(GetRazorView<AreaRegistration>("Content/Index.cshtml"));
        }
    }
}