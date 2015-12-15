using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;
using Frapid.Account.DTO;
using Frapid.Messaging;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.Account.Messaging
{
    public class ResetEmail
    {
        private readonly Reset _resetDetails;

        public ResetEmail(Reset reset)
        {
            _resetDetails = reset;
        }

        private string GetTemplate()
        {
            string path = "~/Catalogs/{catalog}/Areas/Frapid.Account/EmailTemplates/password-reset.html";
            path = path.Replace("{catalog}", AppUsers.GetCatalog());
            path = HostingEnvironment.MapPath(path);

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            return path != null ? File.ReadAllText(path, Encoding.UTF8) : string.Empty;
        }

        private string ParseTemplate(string template)
        {
            string siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string link = siteUrl + "/account/reset/confirm?token=" + _resetDetails.RequestId;

            string parsed = template.Replace("{{Name}}", _resetDetails.Name);
            parsed = parsed.Replace("{{EmailAddress}}", _resetDetails.Email);
            parsed = parsed.Replace("{{ResetLink}}", link);
            parsed = parsed.Replace("{{SiteUrl}}", siteUrl);

            return parsed;
        }

        private EmailQueue GetEmail(string catalog, Reset model, string subject, string message)
        {
            Config config = new Config(catalog);

            return new EmailQueue
            {
                AddedOn = DateTime.Now,
                FromName = model.Name,
                ReplyTo = model.Email,
                Subject = subject,
                Message = message,
                SendTo = config.FromEmail
            };
        }

        public async Task SendAsync()
        {
            string template = GetTemplate();
            string parsed = ParseTemplate(template);
            string subject = "Your Password Reset Link for " + HttpContext.Current.Request.Url.Authority;

            var catalog = AppUsers.GetCatalog();
            var email = this.GetEmail(catalog, this._resetDetails, subject, parsed);            
            var queue = new MailQueueManager(catalog, email);
            queue.Add();
            await queue.ProcessMailQueueAsync(EmailProcessor.GetDefault());
        }
    }
}