using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class MenuItemController : DashboardController
    {
        [Route("dashboard/website/menus/items")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuItem/Index.cshtml"));
        }
    }
}