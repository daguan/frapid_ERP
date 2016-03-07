using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Messaging.DAL;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Smtp;

namespace Frapid.Messaging
{
    public class MailQueueManager
    {
        public MailQueueManager()
        {
        }

        public MailQueueManager(string database, EmailQueue mail)
        {
            this.Database = database;
            this.Email = mail;
        }

        public EmailQueue Email { get; set; }
        public string Database { get; set; }

        public void Add()
        {
            if (!this.IsEnabled())
            {
                return;
            }

            MailQueue.AddToQueue(this.Database, this.Email);
        }

        private bool IsEnabled()
        {
            var processor = EmailProcessor.GetDefault(this.Database);
            return processor != null && processor.IsEnabled;
        }

        public async Task ProcessMailQueueAsync(IEmailProcessor processor)
        {
            IEnumerable<EmailQueue> queue = MailQueue.GetMailInQueue(this.Database).ToList();
            var config = new Config(this.Database);

            if (this.IsEnabled())
            {
                foreach (var mail in queue)
                {
                    var message = EmailHelper.GetMessage(config, mail);
                    var attachments = mail.Attachments?.Split(',').ToArray();

                    bool success = await processor.SendAsync(message, false, attachments);

                    if (!success)
                    {
                        continue;
                    }

                    mail.Delivered = true;
                    mail.DeliveredOn = DateTime.UtcNow;


                    MailQueue.SetSuccess(this.Database, mail.QueueId);
                }
            }
        }
    }
}