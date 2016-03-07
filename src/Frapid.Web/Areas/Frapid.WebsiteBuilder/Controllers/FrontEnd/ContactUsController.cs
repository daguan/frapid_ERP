using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Emails;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    [AntiForgery]
    public class ContactUsController : WebsiteBuilderController
    {
        [Route("contact-us")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new ContactUs();
            var contacts = Contacts.GetContacts();
            model.Contacts = contacts;
            return this.View(this.GetRazorView<AreaRegistration>("ContactUs/Index.cshtml"), model);
        }

        [Route("contact-us")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendEmailAsync(ContactForm model)
        {
            model.Subject = "Contact Form : " + model.Subject;
            string tenant = AppUsers.GetTenant();
            await new ContactUsEmail().SendAsync(tenant, model);
            Thread.Sleep(1000);
            return this.Json("OK");
        }
    }
}