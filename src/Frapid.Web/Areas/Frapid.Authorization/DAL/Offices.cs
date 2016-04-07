using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DTO;
using Frapid.Configuration;

namespace Frapid.Authorization.DAL
{
    public static class Offices
    {
        public static IEnumerable<Office> GetOffices()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<Office>(sql => sql).OrderBy(x => x.OfficeId);
            }
        }
    }
}