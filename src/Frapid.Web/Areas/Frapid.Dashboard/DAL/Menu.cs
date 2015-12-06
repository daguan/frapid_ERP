using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DAL
{
    public static class Menu
    {
        public static IEnumerable<DTO.Menu> Get()
        {
            const string sql = "SELECT * FROM config.menus;";
            return Factory.Get<DTO.Menu>(AppUsers.GetCatalog(), sql);
        }
    }
}