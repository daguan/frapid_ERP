using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.Controllers.Backend
{
    public class RoleController : DashboardController
    {
        [Route("dashboard/account/roles")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Role/Index.cshtml"));
        }
    }
}