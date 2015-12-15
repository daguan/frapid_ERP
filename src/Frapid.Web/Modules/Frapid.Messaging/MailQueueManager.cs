using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.DataAccess;
using Frapid.Messaging.DAL;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;

namespace Frapid.Messaging
{
    public class MailQueueManager
    {
        public MailQueueManager()
        {
        }

        public MailQueueManager(string catalog, EmailQueue mail)
        {
            Catalog = catalog;
            Email = mail;
        }

        public EmailQueue Email { get; set; }
        public string Catalog { get; set; }

        public void Add()
        {
            if (!IsEnabled())
            {
                return;
            }

            MailQueue.AddToQueue(Catalog, Email);
        }

        private bool IsEnabled()
        {
            Config config = new Config(Catalog);
            return config.Enabled;
        }

        public async Task ProcessMailQueueAsync(IEmailProcessor processor)
        {
            IEnumerable<EmailQueue> queue = MailQueue.GetMailInQueue(Catalog).ToList();
            Config config = new Config(Catalog);

            if (IsEnabled())
            {
                foreach (EmailQueue mail in queue)
                {
                    EmailMessage message = EmailHelper.GetMessage(mail);
                    SmtpHost host = EmailHelper.GetSmtpHost(config);
                    ICredentials credentials = EmailHelper.GetCredentials(config);
                    string[] attachments = mail.Attachments?.Split(',').ToArray();

                    bool success = await processor.SendAsync(message, host, credentials, false, attachments);

                    if (!success)
                    {
                        continue;
                    }

                    mail.Delivered = true;
                    mail.DeliveredOn = DateTime.UtcNow;


                    MailQueue.SetSuccess(this.Catalog, mail.QueueId);
                }
            }
        }
    }
}