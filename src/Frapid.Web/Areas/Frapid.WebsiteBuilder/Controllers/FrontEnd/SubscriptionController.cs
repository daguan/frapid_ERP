using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Emails;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class SubscriptionController : WebsiteBuilderController
    {
        private const string TokenKey = "Token";

        [Route("subscription/add")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddAsync(Subscribe model)
        {
            string token = this.Session[TokenKey].ToString();

            //ConfirmEmailAddress is a honeypot field
            if (token != model.TokenHidden || !string.IsNullOrWhiteSpace(model.ConfirmEmailAddress))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string catalog = AppUsers.GetCatalog();

            if (EmailSubscriptions.Add(catalog, model.EmailAddress))
            {
                var email = new SubscriptionWelcomeEmail();
                await email.SendAsync(catalog, model);
            }

            return Json("OK");
        }

        [Route("subscription/remove")]
        [AllowAnonymous]
        public ActionResult Remove()
        {
            string token = Guid.NewGuid().ToString();
            this.Session[TokenKey] = token;
            this.ViewBag.Token = token;
            return this.View(this.GetRazorView<AreaRegistration>("Subscription/Remove.cshtml"));
        }

        [Route("subscription/remove")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RemoveAsync(Subscribe model)
        {
            string token = this.Session[TokenKey].ToString();

            if (token != model.TokenHidden || !string.IsNullOrWhiteSpace(model.ConfirmEmailAddress))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string catalog = AppUsers.GetCatalog();

            if (EmailSubscriptions.Remove(catalog, model.EmailAddress))
            {
                var email = new SubscriptionRemovedEmail();
                await email.SendAsync(catalog, model);
            }

            return Json("OK");
        }
    }
}