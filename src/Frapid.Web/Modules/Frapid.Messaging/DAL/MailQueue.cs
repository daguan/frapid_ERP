using System.Collections.Generic;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    internal static class MailQueue
    {
        public static void AddToQueue(string catalog, EmailQueue queue)
        {
            Factory.Insert(catalog, queue, "congif.email_queue", "queue_id");
        }

        public static IEnumerable<EmailQueue> GetMailInQueue(string catlog)
        {
            const string sql = "SELECT * FROM congif.email_queue WHERE NOT delivered AND NOT canceled;";
            return Factory.Get<EmailQueue>(catlog, sql);
        }
    }
}