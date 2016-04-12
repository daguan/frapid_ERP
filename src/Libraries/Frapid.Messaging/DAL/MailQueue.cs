using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    internal static class MailQueue
    {
        public static void AddToQueue(string database, EmailQueue queue)
        {
            Factory.Insert(database, queue, "config.email_queue", "queue_id");
        }

        public static IEnumerable<EmailQueue> GetMailInQueue(string database)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(database)).GetDatabase())
            {
                return db.FetchBy<EmailQueue>(sql => sql
                    .Where(u => !u.IsTest && !u.Delivered && !u.Canceled && u.SendOn <= DateTimeOffset.UtcNow));
            }
        }

        public static void SetSuccess(string database, long queueId)
        {
            dynamic poco = new ExpandoObject();
            poco.queue_id = queueId;
            poco.delivered = true;
            poco.delivered_on = DateTimeOffset.UtcNow;

            Factory.Update(database, poco, queueId, "config.email_queue", "queue_id");
        }
    }
}