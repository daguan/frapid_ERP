using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class App
    {
        public static async Task<IEnumerable<DTO.App>> GetAsync(string tenant, int userId, int officeId, string cultureCode)
        {
            const string sql = "SELECT * FROM auth.get_apps(@0, @1, @2);";
            return await Factory.GetAsync<DTO.App>(tenant, sql, userId, officeId, cultureCode);
        }
    }
}