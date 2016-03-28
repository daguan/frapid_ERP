using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Frapid.Messaging;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;
using Frapid.Messaging.Smtp;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Emails
{
    public class ContactUsEmail
    {
        private const string TemplatePath = "~/Tenants/{0}/Areas/Frapid.WebsiteBuilder/EmailTemplates/contact-us.html";

        private string ConvertLines(string message)
        {
            return Regex.Replace(message, @"\r\n?|\n", "<br />");
        }

        private string GetMessage(string tenant, ContactForm model)
        {
            string fallback = model.Email + " wrote : <br/><br/>" + this.ConvertLines(model.Message);

            string file = HostingEnvironment.MapPath(string.Format(CultureInfo.InvariantCulture, TemplatePath, tenant));

            if (file == null || !File.Exists(file))
            {
                return fallback;
            }

            string message = File.ReadAllText(file, Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(message))
            {
                return fallback;
            }

            message = message.Replace("{{Email}}", model.Email);
            message = message.Replace("{{Name}}", model.Name);
            message = message.Replace("{{Message}}", this.ConvertLines(model.Message));

            return message;
        }

        private string GetEmails(string tenant, int contactId)
        {
            var config = EmailProcessor.GetDefaultConfig(tenant);
            var contact = DAL.Contacts.GetContact(contactId);

            if (contact == null)
            {
                return config.FromEmail;
            }

            return !string.IsNullOrWhiteSpace(contact.Recipients) ? contact.Recipients : contact.Email;
        }

        private EmailQueue GetEmail(string tenant, ContactForm model)
        {
            return new EmailQueue
            {
                AddedOn = DateTime.UtcNow,
                FromName = model.Name,
                ReplyTo = model.Email,
                Subject = model.Subject,
                Message = this.GetMessage(tenant, model),
                SendTo = this.GetEmails(tenant, model.ContactId)
            };
        }

        public async Task SendAsync(string tenant, ContactForm model)
        {
            try
            {
                var email = this.GetEmail(tenant, model);
                var manager = new MailQueueManager(tenant, email);
                manager.Add();

                var processor = EmailProcessor.GetDefault(tenant);

                if (string.IsNullOrWhiteSpace(email.ReplyTo))
                {
                    email.ReplyTo = processor.Config.FromEmail;
                }

                await manager.ProcessMailQueueAsync(processor);
            }
            catch
            {
                throw new HttpException(500, "Internal Server Error");
            }
        }
    }
}