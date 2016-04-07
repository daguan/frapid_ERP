using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DTO;
using Frapid.Configuration;

namespace Frapid.Authorization.DAL
{
    public static class Users
    {
        public static IEnumerable<User> GetUsers()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(x=>x.Status));
            }
        }
    }
}