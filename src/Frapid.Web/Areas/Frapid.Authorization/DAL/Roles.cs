using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Roles
    {
        public static async Task<IEnumerable<Role>> GetRolesAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Role>().OrderByDescending(x => x.RoleId).ToListAsync().ConfigureAwait(false);
            }
        }
    }
}