using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.Caching;
using Frapid.Areas.CSRF;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Emails;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    [AntiForgery]
    public class SubscriptionController: WebsiteBuilderController
    {
        [Route("subscription/add")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddAsync(Subscribe model)
        {
            //ConfirmEmailAddress is a honeypot field
            if(!string.IsNullOrWhiteSpace(model.ConfirmEmailAddress))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string tenant = AppUsers.GetTenant();

            if(await EmailSubscriptions.AddAsync(tenant, model.EmailAddress))
            {
                var email = new SubscriptionWelcomeEmail();
                await email.SendAsync(tenant, model);
            }

            await Task.Delay(1000);
            return this.Ok();
        }

        [Route("subscription/remove")]
        [AllowAnonymous]
        [FrapidOutputCache(ProfileName = "RemoveSubscription")]
        public ActionResult Remove()
        {
            return this.View(this.GetRazorView<AreaRegistration>("Subscription/Remove.cshtml"));
        }

        [Route("subscription/remove")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RemoveAsync(Subscribe model)
        {
            if(!string.IsNullOrWhiteSpace(model.ConfirmEmailAddress))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string tenant = AppUsers.GetTenant();

            if(await EmailSubscriptions.RemoveAsync(tenant, model.EmailAddress))
            {
                var email = new SubscriptionRemovedEmail();
                await email.SendAsync(tenant, model);
            }

            await Task.Delay(1000);
            return this.Ok();
        }
    }
}