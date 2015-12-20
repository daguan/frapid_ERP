using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.Messaging;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;
using Frapid.WebsiteBuilder.Entities;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Emails
{
    public class SubscriptionRemovedEmail
    {
        private const string TemplatePath =
            "~/Catalogs/{0}/Areas/Frapid.WebsiteBuilder/EmailTemplates/email-subscription-removed.html";

        private string GetMessage(string catalog, Subscribe model)
        {
            string siteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string domain = HttpContext.Current.Request.Url.Host;

            string file = HostingEnvironment.MapPath(string.Format(CultureInfo.InvariantCulture, TemplatePath, catalog));

            if (file == null || !File.Exists(file))
            {
                return string.Empty;
            }

            string message = File.ReadAllText(file, Encoding.UTF8);

            message = message.Replace("{{Domain}}", domain);
            message = message.Replace("{{SiteUrl}}", siteUrl);
            message = message.Replace("{{Email}}", model.EmailAddress);

            return message;
        }

        private EmailQueue GetEmail(string catalog, Subscribe model)
        {
            var config = new Config(catalog);
            string domain = HttpContext.Current.Request.Url.Host;
            string subject = string.Format(CultureInfo.InvariantCulture, "You are now unsubscribed on {0}", domain);

            return new EmailQueue
            {
                AddedOn = DateTime.Now,
                FromName = config.FromName,
                ReplyTo = config.FromEmail,
                Subject = subject,
                Message = this.GetMessage(catalog, model),
                SendTo = model.EmailAddress
            };
        }

        public async Task SendAsync(string catalog, Subscribe model)
        {
            try
            {
                var email = this.GetEmail(catalog, model);
                var manager = new MailQueueManager(catalog, email);
                manager.Add();

                await manager.ProcessMailQueueAsync(EmailProcessor.GetDefault());
            }
            catch
            {
                throw new HttpException(500, "Internal Server Error");
            }
        }
    }
}