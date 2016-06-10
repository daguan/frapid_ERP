using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Entities
    {
        public static async Task<IEnumerable<EntityView>> GetAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<EntityView>().ToListAsync().ConfigureAwait(false);
            }
        }
    }
}