using System.Collections.Generic;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class App
    {
        public static IEnumerable<DTO.App> Get(string tenant, int userId, int officeId, string cultureCode)
        {
            const string sql = "SELECT * FROM auth.get_apps(@0, @1, @2);";
            return Factory.Get<DTO.App>(tenant, sql, userId, officeId, cultureCode);
        }
    }
}