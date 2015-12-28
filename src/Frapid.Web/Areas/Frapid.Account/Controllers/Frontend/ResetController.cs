using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.InputModels;
using Frapid.Areas;
using Frapid.WebsiteBuilder.Controllers;
using Reset = Frapid.Account.ViewModels.Reset;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class ResetController : WebsiteBuilderController
    {
        [Route("account/reset")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View(this.GetRazorView<AreaRegistration>("Reset/Index.cshtml"), new Reset());
        }

        [Route("account/reset")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> IndexAsync(ResetInfo model)
        {
            var token = this.Session["Token"];
            if (token == null)
            {
                return this.Redirect("/");
            }

            if (model.Token != token.ToString())
            {
                return this.Redirect("/");
            }

            model.Browser = this.RemoteUser.Browser;
            model.IpAddress = this.RemoteUser.IpAddress;

            if (DAL.ResetRequests.HasActiveResetRequest(model.Email))
            {
                return this.Json(true);
            }

            var result = DAL.ResetRequests.Request(model);

            if (result.UserId <= 0)
            {
                return this.Redirect("/");
            }


            var email = new ResetEmail(result);
            await email.SendAsync();
            return this.Json(true);
        }

        [Route("account/reset/validate-email")]
        [HttpPost]
        [AllowAnonymous]       
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? this.Json(true) : this.Json(!Registrations.HasAccount(email));
        }

        [Route("account/reset/email-sent")]
        [AllowAnonymous]
        public ActionResult ResetEmailSent()
        {
            return this.View(this.GetRazorView<AreaRegistration>("Reset/ResetEmailSent.cshtml"));
        }

        [Route("account/reset/confirm")]
        [AllowAnonymous]
        public ActionResult Do(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return this.Redirect("/site/404");
            }

            var reset = DAL.ResetRequests.GetIfActive(token);

            if (reset == null)
            {
                return this.Redirect("/site/404");
            }

            return this.View(this.GetRazorView<AreaRegistration>("Reset/Do.cshtml"));
        }

        [Route("account/reset/confirm")]
        [HttpPost]
        [AllowAnonymous]        
        public ActionResult Do()
        {
            string token = this.Request.QueryString["token"];
            string password = this.Request.QueryString["password"];

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password))
            {
                return this.Json(false);
            }

            var reset = DAL.ResetRequests.GetIfActive(token);
            if (reset != null)
            {
                DAL.ResetRequests.CompleteReset(token, password);
                return this.Json(true);
            }

            return this.Json(false);
        }
    }
}