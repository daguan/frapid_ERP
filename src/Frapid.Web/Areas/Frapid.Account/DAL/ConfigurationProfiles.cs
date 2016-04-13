using System.Linq;
using Frapid.Account.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Account.DAL
{
    public static class ConfigurationProfiles
    {
        public static ConfigurationProfile GetActiveProfile(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<ConfigurationProfile>(sql => sql.Where(u => u.IsActive)).FirstOrDefault();
            }
        }
    }
}