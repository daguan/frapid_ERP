using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DTO;
using Frapid.Configuration;

namespace Frapid.Authorization.DAL
{
    public static class Entities
    {
        public static IEnumerable<EntityView> Get()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<EntityView>(sql => sql);
            }
        }
    }
}