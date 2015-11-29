using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Authentication.DAL;
using Frapid.Authentication.Messaging;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Authentication.Controllers
{
    public class EmailConfirmationController : WebsiteBuilderController
    {
        [Route("account/sign-up/confirmation-email-sent")]
        public ActionResult SignUpConfirmationEmailSent()
        {
            return View(GetRazorView<AreaRegistration>("Confirmation/EmailSent.cshtml"));
        }

        [Route("account/sign-up/confirm")]
        public async Task<ActionResult> ConfirmAccountAsync(string token)
        {
            Guid id = token.To<Guid>();

            if (Registration.ConfirmRegistration(id))
            {
                DTO.Registration registration = Registration.Get(id);
                WelcomeEmail email = new WelcomeEmail(registration);
                await email.SendAsync();

                return View(GetRazorView<AreaRegistration>("Confirmation/Welcome.cshtml"));
            }

            return View(GetRazorView<AreaRegistration>("Confirmation/InvalidToken.cshtml"));
        }

        [Route("account/sign-up/validate-email")]
        [HttpPost]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? Json(true) : Json(!Registration.EmailExists(email));
        }
    }
}