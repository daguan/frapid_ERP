using System.Linq;
using Frapid.Configuration;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static SmtpConfig GetConfig(string database)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(database)).GetDatabase())
            {
                return db.FetchBy<SmtpConfig>(sql => sql.Where(u => u.Enabled && u.IsDefault)).FirstOrDefault();
            }
        }
    }
}