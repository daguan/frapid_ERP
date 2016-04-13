using System.Collections.Generic;
using System.Linq;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Authorization.DAL
{
    public static class Offices
    {
        public static IEnumerable<Office> GetOffices(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Office>(sql => sql).OrderBy(x => x.OfficeId);
            }
        }
    }
}