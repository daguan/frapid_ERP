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
    public class MenuAccessPolicyController: DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/user-policy")]
        [MenuPolicy]
        public async Task<ActionResult> UserPolicyAsync()
        {
            var model = await MenuAccessPolicyModel.GetAsync();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("MenuPolicy/Policy.cshtml"), model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/menu-access/user-policy/{officeId}/{userId}")]
        public async Task<ActionResult> GetPolicyAsync(int officeId, int userId)
        {
            var model = await MenuAccessPolicyModel.GetAsync(officeId, userId);
            return this.Ok(model);
        }


        [RestrictAnonymous]
        [HttpPut]
        [Route("dashboard/authorization/menu-access/user-policy")]
        public async Task<ActionResult> SavePolicyAsync(UserMenuPolicyInfo model)
        {
            if(!ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            await MenuAccessPolicyModel.SaveAsync(model);
            return this.Ok("OK");
        }
    }
}