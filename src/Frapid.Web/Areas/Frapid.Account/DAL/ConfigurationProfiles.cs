using System.Threading.Tasks;
using Frapid.Account.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Account.DAL
{
    public static class ConfigurationProfiles
    {
        public static async Task<ConfigurationProfile> GetActiveProfileAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<ConfigurationProfile>().Where(u => u.IsActive).FirstOrDefaultAsync();
            }
        }
    }
}