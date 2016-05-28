using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.InputModels;
using Frapid.Account.Models;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class ChangePasswordController : WebsiteBuilderController
    {
        [Route("account/change-password")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            if (RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            return this.View(this.GetRazorView<AreaRegistration>("ChangePassword/Index.cshtml"));
        }

        [Route("account/change-password")]
        [RestrictAnonymous]
        [HttpPost]
        public async Task<ActionResult> PostAsync(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            bool result = await ChangePasswordModel.ChangePasswordAsync(model, this.RemoteUser);
            return this.Ok(result);
        }

        [Route("account/change-password/success")]
        [RestrictAnonymous]
        public ActionResult Success()
        {
            if (RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            return this.View(this.GetRazorView<AreaRegistration>("ChangePassword/Success.cshtml"));
        }
    }
}