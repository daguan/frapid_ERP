using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.InputModels;
using Frapid.Account.ViewModels;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.CSRF;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class ResetController: WebsiteBuilderController
    {
        [Route("account/reset")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if(RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            return this.View(this.GetRazorView<AreaRegistration>("Reset/Index.cshtml"), new Reset());
        }

        [Route("account/reset")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> IndexAsync(ResetInfo model)
        {
            var token = this.Session["Token"];
            if(token == null)
            {
                return this.Redirect("/");
            }

            if(model.Token != token.ToString())
            {
                return this.Redirect("/");
            }

            model.Browser = this.RemoteUser.Browser;
            model.IpAddress = this.RemoteUser.IpAddress;
            string tenant = AppUsers.GetTenant();

            if(await ResetRequests.HasActiveResetRequestAsync(tenant, model.Email))
            {
                return this.Json(true);
            }

            var result = await ResetRequests.RequestAsync(tenant, model);

            if(result.UserId <= 0)
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
        public async Task<ActionResult> ValidateEmailAsync(string email)
        {
            await Task.Delay(1000);
            string tenant = AppUsers.GetTenant();

            return string.IsNullOrWhiteSpace(email) ? this.Json(true) : this.Json(!await Registrations.HasAccountAsync(tenant, email));
        }

        [Route("account/reset/email-sent")]
        [AllowAnonymous]
        public ActionResult ResetEmailSent()
        {
            if(RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            return this.View(this.GetRazorView<AreaRegistration>("Reset/ResetEmailSent.cshtml"));
        }

        [Route("account/reset/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> DoAsync(string token)
        {
            if(RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            if(string.IsNullOrWhiteSpace(token))
            {
                return this.Redirect("/site/404");
            }

            string tenant = AppUsers.GetTenant();

            var reset = await ResetRequests.GetIfActiveAsync(tenant, token);

            if(reset == null)
            {
                return this.Redirect("/site/404");
            }

            return this.View(this.GetRazorView<AreaRegistration>("Reset/Do.cshtml"));
        }

        [Route("account/reset/confirm")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoAsync()
        {
            string token = this.Request.QueryString["token"];
            string password = this.Request.QueryString["password"];

            if(string.IsNullOrWhiteSpace(token) ||
               string.IsNullOrWhiteSpace(password))
            {
                return this.Json(false);
            }

            string tenant = AppUsers.GetTenant();

            var reset = await ResetRequests.GetIfActiveAsync(tenant, token);
            if(reset != null)
            {
                await ResetRequests.CompleteResetAsync(tenant, token, password);
                return this.Json(true);
            }

            return this.Json(false);
        }
    }
}