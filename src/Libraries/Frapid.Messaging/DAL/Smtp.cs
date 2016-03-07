using System.Linq;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static SmtpConfig GetConfig(string database)
        {
            const string sql = "SELECT * FROM config.smtp_configs WHERE enabled AND is_default LIMIT 1;";
            return Factory.Get<DTO.SmtpConfig>(database, sql).FirstOrDefault();
        }
    }
}