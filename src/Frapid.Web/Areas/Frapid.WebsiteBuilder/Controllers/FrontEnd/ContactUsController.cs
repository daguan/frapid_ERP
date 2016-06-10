using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.CSRF;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Emails;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    [AntiForgery]
    public class ContactUsController: WebsiteBuilderController
    {
        [Route("contact-us")]
        [AllowAnonymous]
        public async Task<ActionResult> IndexAsync()
        {
            string tenant = AppUsers.GetTenant();
            var model = new ContactUs();

            var contacts = await Contacts.GetContactsAsync(tenant).ConfigureAwait(false);
            model.Contacts = contacts;
            return this.View(this.GetRazorView<AreaRegistration>("ContactUs/Index.cshtml", this.Tenant), model);
        }

        [Route("contact-us")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendEmailAsync(ContactForm model)
        {
            model.Subject = "Contact Form : " + model.Subject;
            string tenant = AppUsers.GetTenant();
            await new ContactUsEmail().SendAsync(tenant, model).ConfigureAwait(false);
            await Task.Delay(1000).ConfigureAwait(false);
            return this.Json("OK");
        }
    }
}