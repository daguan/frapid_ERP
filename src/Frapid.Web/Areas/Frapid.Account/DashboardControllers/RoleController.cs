using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.DashboardControllers
{
    public class RoleController : DashboardController
    {
        [Route("dashboard/account/roles")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Role/Index.cshtml"));
        }
    }
}