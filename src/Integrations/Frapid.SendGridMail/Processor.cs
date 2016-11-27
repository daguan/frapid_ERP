using System;
using System.Threading.Tasks;
using Frapid.Messaging;
using Frapid.Messaging.DTO;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Frapid.SendGridMail
{
    public class Processor : IEmailProcessor
    {
        public IEmailConfig Config { get; set; }
        public bool IsEnabled { get; set; }

        public void InitializeConfig(string database)
        {
            var config = SendGridMail.Config.Get(database);
            this.Config = config;

            this.IsEnabled = this.Config.Enabled;

            if (!this.IsEnabled)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(config.ApiKey) ||
               string.IsNullOrWhiteSpace(config.ApiUser))
            {
                this.IsEnabled = false;
            }
        }

        public async Task<bool> SendAsync(EmailMessage email)
        {
            return await this.SendAsync(email, false, null).ConfigureAwait(false);
        }

        public async Task<bool> SendAsync(EmailMessage email, bool deleteAttachmentes, params string[] attachments)
        {
            var config = this.Config as Config;

            if (config == null)
            {
                email.Status = Status.Cancelled;
                return false;
            }

            try
            {
                email.Status = Status.Executing;

                var personalization = new Personalization
                {
                    Subject = email.Subject
                };


                var message = new Mail
                {
                    From = new Email(email.FromEmail, email.FromName),
                    Subject = email.Subject
                };

                if (!string.IsNullOrWhiteSpace(email.ReplyToEmail))
                {
                    message.ReplyTo = new Email(email.ReplyToEmail, email.ReplyToName);
                }


                foreach (var address in email.SentTo.Split(','))
                {
                    personalization.AddTo(new Email(address.Trim()));
                }


                message.AddPersonalization(personalization);

                var content = new Content();
                content.Value = email.Message;

                if (email.IsBodyHtml)
                {
                    content.Type = "text/html";
                }
                else
                {
                    content.Type = "text/plain";
                }

                message.AddContent(content);

                message = AttachmentHelper.AddAttachments(message, attachments);

                var sg = new SendGridAPIClient(config.ApiKey, "https://api.sendgrid.com");
                dynamic response = await sg.client.mail.send.post(requestBody: message.Get());

                System.Net.HttpStatusCode status = response.StatusCode;

                switch (status)
                {
                    case System.Net.HttpStatusCode.OK:
                    case System.Net.HttpStatusCode.Created:
                    case System.Net.HttpStatusCode.Accepted:
                    case System.Net.HttpStatusCode.NoContent:
                        email.Status = Status.Completed;
                        break;
                    default:
                        email.Status = Status.Failed;
                        break;
                }

                return true;
            }
            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                email.Status = Status.Failed;
                Log.Warning(@"Could not send email to {To} using SendGrid API. {Ex}. ", email.SentTo, ex);
            }
            finally
            {
                if (deleteAttachmentes)
                {
                    FileHelper.DeleteFiles(attachments);
                }
            }

            return false;
        }
    }
}