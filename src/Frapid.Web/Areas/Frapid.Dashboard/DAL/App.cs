using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class App
    {
        public static IEnumerable<DTO.App> Get(int userId, int officeId, string cultureCode)
        {
            const string sql = "SELECT * FROM auth.get_apps(@0, @1, @2);";
            return Factory.Get<DTO.App>(AppUsers.GetTenant(), sql, userId, officeId, cultureCode);
        }
    }
}