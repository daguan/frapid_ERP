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

        public MailQueueManager(string catalog, string message, string attachments, string sendTo, string subject)
        {
            Catalog = catalog;
            Message = message;
            Attachments = attachments;
            SendTo = sendTo;
            Subject = subject;
        }

        public string Message { get; }
        public string Attachments { get; }
        public string SendTo { get; }
        public string Subject { get; }
        public string Catalog { get; set; }

        public void Add()
        {
            if (!IsEnabled())
            {
                return;
            }

            EmailQueue queue = new EmailQueue
            {
                Subject = Subject,
                SendTo = SendTo,
                Attachments = Attachments,
                Message = Message,
                AddedOn = DateTime.UtcNow
            };

            MailQueue.AddToQueue(Catalog, queue);
        }

        private bool IsEnabled()
        {
            Config config = new Config(Catalog);
            return config.Enabled;
        }

        public async Task ProcessMailQueueAsync()
        {
            IEnumerable<EmailQueue> queue = MailQueue.GetMailInQueue(Catalog);

            if (IsEnabled())
            {
                foreach (EmailQueue mail in queue)
                {
                    Processor processor = new Processor(Catalog);
                    bool success =
                        await
                            processor.SendAsync(mail.SendTo, mail.Subject, mail.Message, false,
                                mail.Attachments.Split(',').ToArray());

                    if (!success)
                    {
                        continue;
                    }

                    mail.Delivered = true;
                    mail.DeliveredOn = DateTime.UtcNow;
                    Factory.Update(Catalog, mail, mail.QueueId, "core.email_queue", "queue_id");
                }
            }
        }
    }
}