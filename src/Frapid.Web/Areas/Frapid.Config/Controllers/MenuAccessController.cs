using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class MenuAccessController : DashboardController
    {
        [Route("dashboard/config/menu-access")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuAccess/Index.cshtml"));
        }
    }
}