using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class ApiAccessController : DashboardController
    {
        [Route("dashboard/config/api-access")]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("ApiAccess/Index.cshtml", this.Tenant));
        }
    }
}