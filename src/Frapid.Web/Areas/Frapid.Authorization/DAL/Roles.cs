using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DTO;
using Frapid.Configuration;

namespace Frapid.Authorization.DAL
{
    public static class Roles
    {
        public static IEnumerable<Role> GetRoles()
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<Role>(sql => sql).OrderByDescending(x => x.RoleId);
            }
        }
    }
}