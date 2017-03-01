using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.Controllers.Backend
{
    public class RoleController : DashboardController
    {
        [Route("dashboard/account/roles")]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Role/Index.cshtml", this.Tenant));
        }
    }
}