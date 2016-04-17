using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.Models;
using Frapid.Account.ViewModels;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class SignUpController : WebsiteBuilderController
    {
        [Route("account/sign-up")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            string tenant = AppUsers.GetTenant();
            var profile = ConfigurationProfiles.GetActiveProfile(tenant);

            if (!profile.AllowRegistration || this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/dashboard");
            }

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/Index.cshtml"));
        }

        [Route("account/sign-up/confirmation-email-sent")]
        [AllowAnonymous]
        public ActionResult EmailSent()
        {
            if (RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/EmailSent.cshtml"));
        }

        [Route("account/sign-up/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmAsync(string token)
        {
            if (RemoteUser.IsListedInSpamDatabase())
            {
                return this.View(this.GetRazorView<AreaRegistration>("ListedInSpamDatabase.cshtml"));
            }

            var id = token.To<Guid>();
            string tenant = AppUsers.GetTenant();

            if (!Registrations.ConfirmRegistration(tenant, id))
            {
                return this.View(this.GetRazorView<AreaRegistration>("SignUp/InvalidToken.cshtml"));
            }

            var registration = Registrations.Get(tenant, id);
            var email = new WelcomeEmail(registration);
            await email.SendAsync();

            return this.View(this.GetRazorView<AreaRegistration>("SignUp/Welcome.cshtml"));
        }

        [Route("account/sign-up/validate-email")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            string tenant = AppUsers.GetTenant();

            return string.IsNullOrWhiteSpace(email) ? this.Json(true) : this.Json(!Registrations.EmailExists(tenant, email));
        }

        [Route("account/sign-up")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync(Registration model)
        {
            bool result = await SignUpModel.SignUp(model, this.RemoteUser);
            return this.Ok(result);
        }
    }
}