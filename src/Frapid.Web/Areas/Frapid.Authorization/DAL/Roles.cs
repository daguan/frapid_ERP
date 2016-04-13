using System.Collections.Generic;
using System.Linq;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Roles
    {
        public static IEnumerable<Role> GetRoles(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Role>(sql => sql).OrderByDescending(x => x.RoleId);
            }
        }
    }
}