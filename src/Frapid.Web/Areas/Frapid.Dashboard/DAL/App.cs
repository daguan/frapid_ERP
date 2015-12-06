using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class App
    {
        public static IEnumerable<DTO.App> Get()
        {
            const string sql = "SELECT * FROM config.apps;";
            return Factory.Get<DTO.App>(AppUsers.GetCatalog(), sql);
        }
    }
}