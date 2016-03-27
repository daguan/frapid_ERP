using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.Account.ViewModels;
using Frapid.ApplicationState.Cache;
using Frapid.Messaging;
using Frapid.Messaging.DTO;

namespace Frapid.Account.Emails
{
    public class WelcomeEmail
    {
        private readonly string _provider;
        private readonly string _template;
        private readonly IUserInfo _user;

        public WelcomeEmail(IUserInfo user, string template = "", string provider = "")
        {
            this._user = user;
            this._provider = provider;
            this._template = string.IsNullOrWhiteSpace(template)
                ? "~/Tenants/{tenant}/Areas/Frapid.Account/EmailTemplates/welcome-email.html"
                : template;
        }

        private string GetTemplate()
        {
            string path = this._template;
            path = path.Replace("{tenant}", AppUsers.GetTenant());
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

            string parsed = template.Replace("{{Name}}", this._user.Name);
            parsed = parsed.Replace("{{Domain}}", HttpContext.Current.Request.Url.Authority);
            parsed = parsed.Replace("{{SiteUrl}}", siteUrl);
            parsed = parsed.Replace("{{ProviderName}}", this._provider);

            return parsed;
        }

        private EmailQueue GetEmail(IUserInfo model, string subject, string message)
        {
            return new EmailQueue
            {
                AddedOn = DateTime.UtcNow,
                FromName = model.Name,
                Subject = subject,
                Message = message,
                SendTo = model.Email,
                SendOn = DateTime.UtcNow
            };
        }

        public async Task SendAsync()
        {
            string template = this.GetTemplate();
            string parsed = this.ParseTemplate(template);
            string subject = "Welcome to " + HttpContext.Current.Request.Url.Authority;
            string tenant = AppUsers.GetTenant();
            var email = this.GetEmail(this._user, subject, parsed);

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