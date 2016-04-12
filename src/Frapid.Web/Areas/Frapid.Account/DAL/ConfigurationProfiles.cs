using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;

namespace Frapid.Account.DAL
{
    public static class ConfigurationProfiles
    {
        public static ConfigurationProfile GetActiveProfile()
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<ConfigurationProfile>(sql => sql.Where(u => u.IsActive)).FirstOrDefault();
            }
        }
    }
}