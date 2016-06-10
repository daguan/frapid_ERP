using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.Models;
using Frapid.Account.ViewModels;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.CSRF;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class SignUpController : WebsiteBuilderController
    {
        [Route("account/sign-up")]
        [AllowAnonymous]
        public async Task<ActionResult> IndexAsync()
        {
            if (RemoteUser.IsListedInSpamDatabase(this.Tenant))
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml", this.Tenant));
            }

            string tenant = AppUsers.GetTenant();
            var profile = await ConfigurationProfiles.GetActiveProfileAsync(tenant).ConfigureAwait(true);

            if (!profile.AllowRegistration ||
                this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/dashboard");
            }

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/Index.cshtml", this.Tenant));
        }

        [Route("account/sign-up/confirmation-email-sent")]
        [AllowAnonymous]
        public ActionResult EmailSent()
        {
            if (RemoteUser.IsListedInSpamDatabase(this.Tenant))
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml", this.Tenant));
            }

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/EmailSent.cshtml", this.Tenant));
        }

        [Route("account/sign-up/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmAsync(string token)
        {
            if (RemoteUser.IsListedInSpamDatabase(this.Tenant))
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml", this.Tenant));
            }

            var id = token.To<Guid>();
            string tenant = AppUsers.GetTenant();

            if (!await Registrations.ConfirmRegistrationAsync(tenant, id).ConfigureAwait(false))
            {
                return this.View(this.GetRazorView<AreaRegistration>("SignUp/InvalidToken.cshtml", this.Tenant));
            }

            var registration = await Registrations.GetAsync(tenant, id).ConfigureAwait(true);
            var email = new WelcomeEmail(registration);
            await email.SendAsync().ConfigureAwait(false);

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/Welcome.cshtml", this.Tenant));
        }

        [Route("account/sign-up/validate-email")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateEmailAsync(string email)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            string tenant = AppUsers.GetTenant();

            return string.IsNullOrWhiteSpace(email)
                ? this.Json(true)
                : this.Json(!await Registrations.EmailExistsAsync(tenant, email).ConfigureAwait(true));
        }

        [Route("account/sign-up")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync(Registration model)
        {
            bool result = await SignUpModel.SignUpAsync(model, this.RemoteUser).ConfigureAwait(true);
            return this.Ok(result);
        }
    }
}