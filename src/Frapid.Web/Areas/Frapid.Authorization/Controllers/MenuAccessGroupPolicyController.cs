using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Authorization.Models;
using Frapid.Authorization.ViewModels;
using Frapid.Dashboard.Controllers;

namespace Frapid.Authorization.Controllers
{
    [AntiForgery]
    public class MenuAccessGroupPolicyController : DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/group-policy")]
        [MenuPolicy]
        public ActionResult GroupPolicy()
        {
            var model = GroupMenuPolicyModel.Get();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuPolicy/GroupPolicy.cshtml"), model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/group-policy/{officeId}/{roleId}")]
        public ActionResult GetGroupPolicy(int officeId, int roleId)
        {
            var model = GroupMenuPolicyModel.Get(officeId, roleId);
            return this.Ok(model);
        }


        [RestrictAnonymous]
        [HttpPut]
        [Route("dashboard/authorization/menu-access/group-policy")]
        public ActionResult SaveGroupPolicy(GroupMenuPolicyInfo model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            GroupMenuPolicyModel.Save(model);
            return this.Ok("OK");
        }
    }
}