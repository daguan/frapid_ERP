using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Authentication.Messaging;
using Frapid.Authentication.ViewModels;
using Frapid.WebsiteBuilder.Controllers;
using Registration = Frapid.Authentication.DAL.Registration;

namespace Frapid.Authentication.Controllers
{
    public class ResetController : WebsiteBuilderController
    {
        [Route("account/reset")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(GetRazorView<AreaRegistration>("Reset/Index.cshtml"), new Reset());
        }

        [Route("account/reset")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> IndexAsync(ResetInputModel model)
        {
            var token = Session["Token"];
            if (token == null)
            {
                return Redirect("/");
            }

            if (model.Token != token.ToString())
            {
                return Redirect("/");
            }

            model.Browser = this.GetRemoteUser().Browser;
            model.IpAddress = this.GetRemoteUser().IpAddress;

            if (DAL.Reset.HasActiveResetRequest(model.Email))
            {
                return Json(true);
            }

            var result = DAL.Reset.Request(model);

            if (result.UserId <= 0)
            {
                return Redirect("/");
            }


            ResetEmail email = new ResetEmail(result);
            await email.SendAsync();
            return Json(true);
        }

        [Route("account/reset/validate-email")]
        [HttpPost]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? Json(true) : Json(!Registration.HasAccount(email));
        }

        [Route("account/reset/email-sent")]
        public ActionResult ResetEmailSent()
        {
            return View(GetRazorView<AreaRegistration>("Confirmation/ResetEmailSent.cshtml"));
        }

    }
}