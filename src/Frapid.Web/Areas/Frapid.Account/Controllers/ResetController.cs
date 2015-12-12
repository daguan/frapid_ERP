using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.InputModels;
using Frapid.Account.Messaging;
using Frapid.Account.ViewModels;
using Frapid.WebsiteBuilder.Controllers;
using Registration = Frapid.Account.DAL.Registration;

namespace Frapid.Account.Controllers
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
        public async Task<ActionResult> IndexAsync(ResetInfo model)
        {
            object token = Session["Token"];
            if (token == null)
            {
                return Redirect("/");
            }

            if (model.Token != token.ToString())
            {
                return Redirect("/");
            }

            model.Browser = this.RemoteUser.Browser;
            model.IpAddress = this.RemoteUser.IpAddress;

            if (DAL.Reset.HasActiveResetRequest(model.Email))
            {
                return Json(true);
            }

            DTO.Reset result = DAL.Reset.Request(model);

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
        [AllowAnonymous]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? Json(true) : Json(!Registration.HasAccount(email));
        }

        [Route("account/reset/email-sent")]
        [AllowAnonymous]
        public ActionResult ResetEmailSent()
        {
            return View(GetRazorView<AreaRegistration>("Reset/ResetEmailSent.cshtml"));
        }

        [Route("account/reset/confirm")]
        [AllowAnonymous]
        public ActionResult Do(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Redirect("/site/404");
            }

            DTO.Reset reset = DAL.Reset.GetIfActive(token);

            if (reset == null)
            {
                return Redirect("/site/404");
            }

            return View(GetRazorView<AreaRegistration>("Reset/Do.cshtml"));
        }

        [Route("account/reset/confirm")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Do()
        {
            string token = Request.QueryString["token"];
            string password = Request.QueryString["password"];

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password))
            {
                return Json(false);
            }

            DTO.Reset reset = DAL.Reset.GetIfActive(token);
            if (reset != null)
            {
                DAL.Reset.CompleteReset(token, password);
                return Json(true);
            }

            return Json(false);
        }
    }
}