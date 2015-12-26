using System.Text;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    [AntiForgery]
    public class SubscriptionEmailTemplateController : DashboardController
    {
        [Route("dashboard/website/subscription/welcome")]
        [RestrictAnonymous]
        public ActionResult Welcome()
        {
            string path = this.GetWelcomeTemplatePath();
            var model = this.GetModel(path);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("SubscriptionEmailTemplate/Welcome.cshtml"),
                model);
        }

        [Route("dashboard/website/subscription/removed")]
        [RestrictAnonymous]
        public ActionResult Removed()
        {
            string path = this.GetSubscriptionRemovedTemplatePath();
            var model = this.GetModel(path);
            return
                this.FrapidView(
                    this.GetRazorView<AreaRegistration>("SubscriptionEmailTemplate/SubscriptionRemoved.cshtml"), model);
        }

        [Route("dashboard/website/subscription/welcome/save")]
        [HttpPost]
        [RestrictAnonymous]
        public ActionResult SaveWelcomeTemplate(Template model)
        {
            string path = this.GetWelcomeTemplatePath();
            System.IO.File.WriteAllText(path, model.Contents, Encoding.UTF8);
            return this.Json("OK");
        }

        [Route("dashboard/website/subscription/removed/save")]
        [HttpPost]
        [RestrictAnonymous]
        public ActionResult SaveSubscriptionRemovedTemplate(Template model)
        {
            string path = this.GetSubscriptionRemovedTemplatePath();
            System.IO.File.WriteAllText(path, model.Contents, Encoding.UTF8);
            return this.Json("OK");
        }

        private string GetSubscriptionRemovedTemplatePath()
        {
            return Configuration.GetWebsiteBuilderPath() + "/EmailTemplates/email-subscription-removed.html";
        }

        private string GetWelcomeTemplatePath()
        {
            return Configuration.GetWebsiteBuilderPath() + "/EmailTemplates/email-subscription-welcome.html";
        }

        private Template GetModel(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new Template();
            }

            string contents = System.IO.File.ReadAllText(path, Encoding.UTF8);

            return new Template
            {
                Contents = contents
            };
        }
    }
}