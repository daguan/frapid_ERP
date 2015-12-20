using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Emails;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class ContactUsController : WebsiteBuilderController
    {
        private const string TokenKey = "Token";

        [Route("contact-us")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new ViewModels.ContactUs();
            var contacts = Contacts.GetContacts();
            model.Contacts = contacts;

            this.Session[TokenKey] = model.Token;

            return this.View(this.GetRazorView<AreaRegistration>("ContactUs/Index.cshtml"), model);
        }

        [Route("contact-us")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendEmailAsync(ContactForm model)
        {
            string token = this.Session[TokenKey].ToString();
            if (token != model.Token)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Subject = "Contact Form : " + model.Subject;
            string catalog = AppUsers.GetCatalog();
            await new ContactUsEmail().SendAsync(catalog, model);
            return this.Json("OK");
        }
    }
}