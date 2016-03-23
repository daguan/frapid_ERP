using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.Models;
using Frapid.Account.ViewModels;
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
            var profile = ConfigurationProfiles.GetActiveProfile();

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
            return this.View(this.GetRazorView<AreaRegistration>("SignUp/EmailSent.cshtml"));
        }

        [Route("account/sign-up/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmAsync(string token)
        {
            var id = token.To<Guid>();

            if (!Registrations.ConfirmRegistration(id))
            {
                return this.View(this.GetRazorView<AreaRegistration>("SignUp/InvalidToken.cshtml"));
            }

            var registration = Registrations.Get(id);
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
            return string.IsNullOrWhiteSpace(email) ? this.Json(true) : this.Json(!Registrations.EmailExists(email));
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