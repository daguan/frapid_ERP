using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class MenuController : DashboardController
    {
        [Route("dashboard/website/menus")]
        [Authorize]
        public ActionResult Index(string alias = "")
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Menu/Index.cshtml"));
        }
    }
}