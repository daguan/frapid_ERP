using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class Menu
    {
        public static async Task<IEnumerable<DTO.Menu>> GetAsync(string tenant, int userId, int officeId, string cultureCode)
        {
            const string sql = "SELECT * FROM auth.get_menu(@0, @1, @2);";
            return await Factory.GetAsync<DTO.Menu>(tenant, sql, userId, officeId, cultureCode);
        }
    }
}