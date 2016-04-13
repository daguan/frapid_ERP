using System.Collections.Generic;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Users
    {
        public static IEnumerable<User> GetUsers(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(x => x.Status));
            }
        }
    }
}