using System.Collections.Generic;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Offices
    {
        public static IEnumerable<Office> GetOffices(string tenant)
        {
            const string sql = "SELECT office_id, office_name FROM core.offices;";
            return Factory.Get<Office>(tenant, sql);
        }
    }
}