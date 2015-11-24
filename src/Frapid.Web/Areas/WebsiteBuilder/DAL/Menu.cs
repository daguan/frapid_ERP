using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace WebsiteBuilder.DAL
{
    public class Menu
    {
        public static IEnumerable<Models.Menu> Get(string menuGroupName)
        {
            const string sql = "SELECT * FROM wb.menus " +
                               "WHERE menu_group_id=" +
                               "(SELECT menu_group_id FROM wb.menu_groups " +
                               "WHERE menu_group_name=@0) " +
                               "ORDER BY sort, menu_id";
            return Factory.Get<Models.Menu>(AppUsers.GetCatalog(), sql, menuGroupName);
        }
    }
}