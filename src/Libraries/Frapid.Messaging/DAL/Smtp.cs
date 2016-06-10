using System.Threading.Tasks;
using Frapid.Configuration.Db;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.DAL
{
    public class Smtp
    {
        public static async Task<SmtpConfig> GetConfigAsync(string tenant)
        {
            using(var db = DbProvider.GetDatabase(tenant))
            {
                return await db.Query<SmtpConfig>().Where(u => u.Enabled && u.IsDefault).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }
    }
}