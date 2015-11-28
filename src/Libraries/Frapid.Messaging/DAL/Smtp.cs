using System.Linq;
using Frapid.DataAccess;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static DTO.Smtp GetConfig(string catalog)
        {
            const string sql = "SELECT * FROM core.smtp_configs WHERE enabled AND is_default LIMIT 1;";
            return Factory.Get<DTO.Smtp>(catalog, sql).FirstOrDefault();
        }
    }
}