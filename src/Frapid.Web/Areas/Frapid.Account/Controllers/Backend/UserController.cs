using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.Controllers.Backend
{
    public class UserController : DashboardController
    {
        [Route("dashboard/account/user-management")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("User/Index.cshtml"));
        }
    }
}