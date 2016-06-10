using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Users
    {
        public static async Task<IEnumerable<User>> GetUsersAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<User>().Where(x => x.Status).ToListAsync().ConfigureAwait(false);
            }
        }
    }
}