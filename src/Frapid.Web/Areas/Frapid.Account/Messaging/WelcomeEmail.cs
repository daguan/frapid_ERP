using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;
using Frapid.Account.DTO;
using Frapid.Account.Models;
using Frapid.Messaging;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;

namespace Frapid.Account.Messaging
{
    public class WelcomeEmail
    {
        private readonly IUserInfo _user;
        private readonly string _template;
        private readonly string _provider;

        public WelcomeEmail(IUserInfo user, string template = "", string provider = "")
        {
            _user = user;
            this._provider = provider;
            this._template = string.IsNullOrWhiteSpace(template) ? "~/Catalogs/{catalog}/Areas/Frapid.Account/EmailTemplates/welcome.html" : template;
        }

        private string GetTemplate()
        {
            string path = this._template;
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

            string parsed = template.Replace("{{Name}}", _user.Name);
            parsed = parsed.Replace("{{Domain}}", HttpContext.Current.Request.Url.Authority);
            parsed = parsed.Replace("{{SiteUrl}}", siteUrl);
            parsed = parsed.Replace("{{ProviderName}}", _provider);

            return parsed;
        }

        private EmailQueue GetEmail(string catalog, IUserInfo model, string subject, string message)
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
            string subject = "Welcome to " + HttpContext.Current.Request.Url.Authority;
            var catalog = AppUsers.GetCatalog();
            var email = this.GetEmail(catalog, this._user, subject, parsed);
            var queue = new MailQueueManager(catalog, email);
            queue.Add();
            await queue.ProcessMailQueueAsync(EmailProcessor.GetDefault());
        }
    }
}