using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Configurations
    {
        public static async Task<DTO.Configuration> GetDefaultConfigurationAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<DTO.Configuration>().Where(c => c.IsDefault).FirstOrDefaultAsync();
            }
        }
    }
}