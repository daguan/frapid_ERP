using System.Collections.Generic;
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
    public class EntityAccessPolicyController : DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/user-policy")]
        [MenuPolicy]
        public ActionResult UserPolicy()
        {
            var model = EntityAccessPolicyModel.Get();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("AccessPolicy/Policy.cshtml"), model);
        }


        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/user-policy/{officeId}/{userId}")]
        public ActionResult GetPolicy(int officeId, int userId)
        {
            var model = EntityAccessPolicyModel.Get(officeId, userId);
            return this.Ok(model);
        }


        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/user-policy/{officeId}/{userId}")]
        [HttpPost]
        public ActionResult SavePolicy(int officeId, int userId, List<AccessPolicyInfo> model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            EntityAccessPolicyModel.Save(officeId, userId, model);
            return this.Ok();
        }
    }
}