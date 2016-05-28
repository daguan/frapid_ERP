using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Authorization.Models;
using Frapid.Authorization.ViewModels;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Authorization.Controllers
{
    [AntiForgery]
    public class MenuAccessGroupPolicyController : DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/group-policy")]
        [MenuPolicy]
        public async Task<ActionResult> GroupPolicyAsync()
        {
            var model = await GroupMenuPolicyModel.GetAsync();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuPolicy/GroupPolicy.cshtml"), model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/group-policy/{officeId}/{roleId}")]
        public async Task<ActionResult> GetGroupPolicyAsync(int officeId, int roleId)
        {
            var model = await GroupMenuPolicyModel.GetAsync(officeId, roleId);
            return this.Ok(model);
        }


        [RestrictAnonymous]
        [HttpPut]
        [Route("dashboard/authorization/menu-access/group-policy")]
        public async Task<ActionResult> SaveGroupPolicyAsync(GroupMenuPolicyInfo model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            await GroupMenuPolicyModel.SaveAsync(model);
            return this.Ok("OK");
        }
    }
}