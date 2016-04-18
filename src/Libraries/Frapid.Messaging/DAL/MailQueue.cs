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
                var sql = new Sql("SELECT * FROM config.email_queue");
                sql.Where("is_test=@0", false);
                sql.Append("AND delivered=@0", false);
                sql.Append("AND canceled=@0", false);
                sql.Append("AND send_on<=" + FrapidDbServer.GetDbTimestampFunction(database));

                return db.Fetch<EmailQueue>(sql);
            }
        }

        public static void SetSuccess(string database, long queueId)
        {
            var sql = new Sql("UPDATE config.email_queue SET");

            sql.Append("delivered=@0, ", true);
            sql.Append("delivered_on=" + FrapidDbServer.GetDbTimestampFunction(database));
            sql.Where("queue_id=@0", queueId);

            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(database), database).GetDatabase())
            {
                db.Execute(sql);
            }
        }
    }
}