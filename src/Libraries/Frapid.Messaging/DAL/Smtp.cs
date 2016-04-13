using System.Linq;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static SmtpConfig GetConfig(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<SmtpConfig>(sql => sql.Where(u => u.Enabled && u.IsDefault)).FirstOrDefault();
            }
        }
    }
}