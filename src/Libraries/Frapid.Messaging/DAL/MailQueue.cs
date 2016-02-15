using System.Collections.Generic;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    internal static class MailQueue
    {
        public static void AddToQueue(string catalog, EmailQueue queue)
        {
            Factory.Insert(catalog, queue, "config.email_queue", "queue_id");
        }

        public static IEnumerable<EmailQueue> GetMailInQueue(string catlog)
        {
            const string sql = "SELECT * FROM config.email_queue WHERE NOT delivered AND NOT canceled AND send_on <= NOW();";
            return Factory.Get<EmailQueue>(catlog, sql);
        }

        public static void SetSuccess(string catalog, long queueId)
        {
            const string sql =
                "UPDATE config.email_queue SET delivered = true, delivered_on = NOW() WHERE queue_id = @0;";
            Factory.NonQuery(catalog, sql, queueId);
        }
    }
}