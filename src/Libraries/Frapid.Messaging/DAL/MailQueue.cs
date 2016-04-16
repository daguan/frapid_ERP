using System;
using System.Collections.Generic;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;
using Frapid.NPoco;

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
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(database), database).GetDatabase())
            {
                return db.FetchBy<EmailQueue>(sql => sql
                    .Where(u => !u.IsTest && !u.Delivered && !u.Canceled && u.SendOn <= DateTimeOffset.UtcNow));
            }
        }

        public static void SetSuccess(string database, long queueId)
        {
            var sql = new Sql("UPDATE config.email_queue SET");
            sql.Append("queue_id=@0, ", queueId);
            sql.Append("delivered=@0, ", true);
            sql.Append("delivered_on=@0, ", DateTimeOffset.UtcNow);
            sql.Where("queue_id=@0, ", queueId);

            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(database), database).GetDatabase())
            {
                db.Execute(sql);
            }
        }
    }
}