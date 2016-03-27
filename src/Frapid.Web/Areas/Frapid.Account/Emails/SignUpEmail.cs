using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Messaging;
using Frapid.Messaging.DTO;

namespace Frapid.Account.Emails
{
    public class SignUpEmail
    {
        private readonly Registration _registration;
        private readonly string _registrationId;

        public SignUpEmail(Registration registration, string registrationId)
        {
            this._registration = registration;
            this._registrationId = registrationId;
        }

        private string GetTemplate()
        {
            string path = $"~/Tenants/{AppUsers.GetTenant()}/Areas/Frapid.Account/EmailTemplates/account-verification.html";

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
            string link = siteUrl + "/account/sign-up/confirm?token=" + this._registrationId;

            string parsed = template.Replace("{{Name}}", this._registration.Name);
            parsed = parsed.Replace("{{EmailAddress}}", this._registration.Email);
            parsed = parsed.Replace("{{VerificationLink}}", link);
            parsed = parsed.Replace("{{SiteUrl}}", siteUrl);

            return parsed;
        }

        private EmailQueue GetEmail(Registration model, string subject, string message)
        {
            return new EmailQueue
            {
                AddedOn = DateTime.UtcNow,
                FromName = model.Name,
                Subject = subject,
                Message = message,
                SendTo = model.Email
            };
        }

        public async Task SendAsync()
        {
            string template = this.GetTemplate();
            string parsed = this.ParseTemplate(template);
            string subject = "Confirm Your Registration at " + HttpContext.Current.Request.Url.Authority;

            string tenant = AppUsers.GetTenant();

            var email = this.GetEmail(this._registration, subject, parsed);

            var processor = EmailProcessor.GetDefault(tenant);
            if (string.IsNullOrWhiteSpace(email.ReplyTo))
            {
                email.ReplyTo = processor.Config.FromEmail;
            }

            var queue = new MailQueueManager(tenant, email);

            queue.Add();
            await queue.ProcessMailQueueAsync(processor);
        }
    }
}