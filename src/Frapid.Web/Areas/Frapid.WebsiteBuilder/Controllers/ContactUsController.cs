using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class ContactUsController : WebsiteBuilderController
    {
        private const string TokenKey = "Token";

        [Route("contact-us")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new ContactUs();
            var contacts = Contacts.GetContacts();
            model.Contacts = contacts;

            Session[TokenKey] = model.Token;

            return View(GetRazorView<AreaRegistration>("ContactUs/Index.cshtml"), model);
        }

        [Route("contact-us")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendEmailAsync(ContactForm model)
        {
            string token = Session[TokenKey].ToString();
            if (token != model.Token)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Subject = "Contact Form : " + model.Subject;
            string catalog = AppUsers.GetCatalog();
            await new Email().SendAsync(catalog, model);
            return Json("OK");
        }
    }
}