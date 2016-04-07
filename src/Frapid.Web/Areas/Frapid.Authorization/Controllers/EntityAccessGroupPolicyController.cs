using System.Collections.Generic;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Authorization.Models;
using Frapid.Authorization.ViewModels;
using Frapid.Dashboard.Controllers;

namespace Frapid.Authorization.Controllers
{
    [AntiForgery]
    public class EntityAccessGroupPolicyController : DashboardController
    {
        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/group-policy")]
        [MenuPolicy]
        public ActionResult GroupPolicy()
        {
            var model = GroupEntityAccessPolicyModel.Get();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("AccessPolicy/GroupPolicy.cshtml"), model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/group-policy/{officeId}/{roleId}")]
        public ActionResult GetGroupPolicy(int officeId, int roleId)
        {
            var model = GroupEntityAccessPolicyModel.Get(officeId, roleId);
            return this.Ok(model);
        }

        [RestrictAnonymous]
        [Route("dashboard/authorization/entity-access/group-policy/{officeId}/{roleId}")]
        [HttpPost]
        public ActionResult SaveGroupPolicy(int officeId, int roleId, List<AccessPolicyInfo> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            GroupEntityAccessPolicyModel.Save(officeId, roleId, model);
            return this.Ok();
        }
    }
}