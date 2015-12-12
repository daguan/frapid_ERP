using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Frapid.Account.DTO;
using Frapid.Account.Exceptions;
using Frapid.Account.Messaging;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Controllers;
using Registration = Frapid.Account.ViewModels.Registration;

namespace Frapid.Account.Controllers
{
    public class SignUpController : WebsiteBuilderController
    {
        [Route("account/sign-up")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ConfigurationProfile profile = DAL.Configuration.GetActiveProfile();

            if (!profile.AllowRegistration || User.Identity.IsAuthenticated)
            {
                return Redirect("/dashboard");
            }

            return View(GetRazorView<AreaRegistration>("SignUp/Index.cshtml"));
        }

        [Route("account/sign-up/confirmation-email-sent")]
        [AllowAnonymous]
        public ActionResult EmailSent()
        {
            return View(GetRazorView<AreaRegistration>("SignUp/EmailSent.cshtml"));
        }

        [Route("account/sign-up/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmAsync(string token)
        {
            Guid id = token.To<Guid>();

            if (!DAL.Registration.ConfirmRegistration(id))
            {
                return View(GetRazorView<AreaRegistration>("SignUp/InvalidToken.cshtml"));
            }

            DTO.Registration registration = DAL.Registration.Get(id);
            WelcomeEmail email = new WelcomeEmail(registration);
            await email.SendAsync();

            return View(GetRazorView<AreaRegistration>("SignUp/Welcome.cshtml"));
        }

        [Route("account/sign-up/validate-email")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? Json(true) : Json(!DAL.Registration.EmailExists(email));
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
            DTO.Registration registration = Mapper.Map<DTO.Registration>(model);
            string registrationId = DAL.Registration.Register(registration).ToString();

            if (string.IsNullOrWhiteSpace(registrationId))
            {
                return Json(false);
            }

            SignUpEmail email = new SignUpEmail(registration, registrationId);
            await email.SendAsync();
            return Json(true);
        }
    }
}