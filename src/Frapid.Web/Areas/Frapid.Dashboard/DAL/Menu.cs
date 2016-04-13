using System.Collections.Generic;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class Menu
    {
        public static IEnumerable<DTO.Menu> Get(string tenant, int userId, int officeId, string cultureCode)
        {
            const string sql = "SELECT * FROM auth.get_menu(@0, @1, @2);";
            return Factory.Get<DTO.Menu>(tenant, sql, userId, officeId, cultureCode);
        }
    }
}