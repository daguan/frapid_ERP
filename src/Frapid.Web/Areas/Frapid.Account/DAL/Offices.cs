using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Account.DTO;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Offices
    {
        public static async Task<IEnumerable<Office>> GetOfficesAsync(string tenant)
        {
            const string sql = "SELECT office_id, office_name FROM core.offices;";
            return await Factory.GetAsync<Office>(tenant, sql);
        }
    }
}