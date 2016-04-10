using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class FlagController : DashboardController
    {
        [Route("dashboard/config/flags")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Flag/Index.cshtml"));
        }
    }
}