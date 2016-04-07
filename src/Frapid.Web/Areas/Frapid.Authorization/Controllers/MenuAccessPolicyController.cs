using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Authorization.Models;
using Frapid.Authorization.ViewModels;
using Frapid.Dashboard.Controllers;

namespace Frapid.Authorization.Controllers
{
    [AntiForgery]
    public class MenuAccessPolicyController : DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/user-policy")]
        [MenuPolicy]
        public ActionResult UserPolicy()
        {
            var model = MenuAccessPolicyModel.Get();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuPolicy/Policy.cshtml"), model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/user-policy/{officeId}/{userId}")]
        public ActionResult GetPolicy(int officeId, int userId)
        {
            var model = MenuAccessPolicyModel.Get(officeId, userId);
            return this.Ok(model);
        }


        [RestrictAnonymous]
        [HttpPut]
        [Route("dashboard/authorization/menu-access/user-policy")]
        public ActionResult SavePolicy(UserMenuPolicyInfo model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            MenuAccessPolicyModel.Save(model);
            return this.Ok("OK");
        }
    }
}