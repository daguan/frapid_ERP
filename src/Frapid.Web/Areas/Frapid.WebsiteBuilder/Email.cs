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
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder
{
    public class Email
    {
        private const string TemplatePath = "~/Catalogs/{0}/Areas/Frapid.WebsiteBuilder/EmailTemplates/contact-us.html";

        private string ConvertLines(string message)
        {
            return Regex.Replace(message, @"\r\n?|\n", "<br />");
        }

        private string GetMessage(string catalog, ContactForm model)
        {
            string fallback = model.Email + " wrote : <br/><br/>" + this.ConvertLines(model.Message);

            string file = HostingEnvironment.MapPath(string.Format(CultureInfo.InvariantCulture, TemplatePath, catalog));

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

        private EmailQueue GetEmail(string catalog, ContactForm model)
        {
            var config = new Config(catalog);

            return new EmailQueue
            {
                AddedOn = DateTime.Now,
                FromName = model.Name,
                ReplyTo = model.Email,
                Subject = model.Subject,
                Message = this.GetMessage(catalog, model),
                SendTo = config.FromEmail
            };
        }

        public async Task SendAsync(string catalog, ContactForm model)
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