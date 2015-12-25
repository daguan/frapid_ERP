using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Frapid.Account.Exceptions;
using Frapid.Account.Messaging;
using Frapid.Areas;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Controllers;
using Registration = Frapid.Account.ViewModels.Registration;

namespace Frapid.Account.Controllers.Frontend
{
    [AntiForgery]
    public class SignUpController : WebsiteBuilderController
    {
        [Route("account/sign-up")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var profile = DAL.ConfigurationProfiles.GetActiveProfile();

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

            if (!DAL.Registrations.ConfirmRegistration(id))
            {
                return this.View(this.GetRazorView<AreaRegistration>("SignUp/InvalidToken.cshtml"));
            }

            var registration = DAL.Registrations.Get(id);
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
            return string.IsNullOrWhiteSpace(email) ? this.Json(true) : this.Json(!DAL.Registrations.EmailExists(email));
        }

        [Route("account/sign-up")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PostAsync(Registration model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordConfirmException("Passwords do not match.");
            }

            if (model.Email != model.ConfirmEmail)
            {
                throw new PasswordConfirmException("Passwords do not match.");
            }

            model.Browser = this.RemoteUser.Browser;
            model.IpAddress = this.RemoteUser.IpAddress;

            Mapper.CreateMap<Registration, DTO.Registration>();
            var registration = Mapper.Map<DTO.Registration>(model);
            string registrationId = DAL.Registrations.Register(registration).ToString();

            if (string.IsNullOrWhiteSpace(registrationId))
            {
                return this.Json(false);
            }

            var email = new SignUpEmail(registration, registrationId);
            await email.SendAsync();
            return this.Json(true);
        }
    }
}