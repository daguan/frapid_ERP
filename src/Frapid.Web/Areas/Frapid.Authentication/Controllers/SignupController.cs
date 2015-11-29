using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Frapid.Authentication.DAL;
using Frapid.Authentication.DTO;
using Frapid.Authentication.Exceptions;
using Frapid.Authentication.Messaging;
using Frapid.WebsiteBuilder.Controllers;
using Registration = Frapid.Authentication.ViewModels.Registration;

namespace Frapid.Authentication.Controllers
{
    public class SignUpController : WebsiteBuilderController
    {
        [Route("account/sign-up")]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            ConfigurationProfile profile = Configuration.GetActiveProfile();

            if (!profile.AllowRegistration || User.Identity.IsAuthenticated)
            {
                return Redirect("/dashboard");
            }

            return View(GetRazorView<AreaRegistration>("Account/SignUp.cshtml"));
        }

        [Route("account/sign-up")]
        [HttpPost]
        public async Task<ActionResult> SignUpAsync(Registration model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordConfirmException("Passwords do not match.");
            }

            if (model.Email != model.ConfirmEmail)
            {
                throw new PasswordConfirmException("Passwords do not match.");
            }

            model.Browser = GetRemoteUser().Browser;
            model.IpAddress = GetRemoteUser().IpAddress;

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