using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class MenuController : DashboardController
    {
        [Route("dashboard/website/menus")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Menu/Index.cshtml"));
        }
    }
}