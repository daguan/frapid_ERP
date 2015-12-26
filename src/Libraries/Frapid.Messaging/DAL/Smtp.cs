using System.Linq;
using Frapid.DataAccess;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static SmtpConfig GetConfig(string catalog)
        {
            const string sql = "SELECT * FROM config.smtp_configs WHERE enabled AND is_default LIMIT 1;";
            return Factory.Get<DTO.SmtpConfig>(catalog, sql).FirstOrDefault();
        }
    }
}