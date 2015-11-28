using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Authentication.Messaging;
using Frapid.Framework.Extensions;
using WebsiteBuilder.Controllers;

namespace Frapid.Authentication.Controllers
{
    public class EmailConfirmationController : WebsiteBuilderController
    {
        [Route("account/sign-up/confirmation-email-sent")]
        public ActionResult SignUpConfirmationEmailSent()
        {
            ViewBag.Layout = "layout.cshtml";
            return View("~/Areas/Frapid.Authentication/Views/Confirmation/EmailSent.cshtml");
        }

        [Route("account/sign-up/confirm")]
        public async Task<ActionResult> ConfirmAccountAsync(string token)
        {
            Guid id = token.To<Guid>();

            if (DAL.Registration.ConfirmRegistration(id))
            {
                DTO.Registration registration = DAL.Registration.Get(id);
                WelcomeEmail email = new WelcomeEmail(registration);
                await email.SendAsync();

                ViewBag.Layout = "layout.cshtml";
                return View("~/Areas/Frapid.Authentication/Views/Confirmation/Welcome.cshtml");
            }

            ViewBag.Layout = "layout.cshtml";
            return View("~/Areas/Frapid.Authentication/Views/Confirmation/InvalidToken.cshtml");
        }


        [Route("account/sign-up/validate-email")]
        [HttpPost]
        public ActionResult ValidateEmail(string email)
        {
            Thread.Sleep(1000);
            return string.IsNullOrWhiteSpace(email) ? Json(true) : Json(!DAL.Registration.EmailExists(email));
        }
    }
}