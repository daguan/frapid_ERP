using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var config = new Config(Catalog);
            return config.Enabled;
        }

        public async Task ProcessMailQueueAsync(IEmailProcessor processor)
        {
            IEnumerable<EmailQueue> queue = MailQueue.GetMailInQueue(Catalog).ToList();
            var config = new Config(Catalog);

            if (IsEnabled())
            {
                foreach (var mail in queue)
                {
                    var message = EmailHelper.GetMessage(config, mail);
                    var host = EmailHelper.GetSmtpHost(config);
                    var credentials = EmailHelper.GetCredentials(config);
                    var attachments = mail.Attachments?.Split(',').ToArray();

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