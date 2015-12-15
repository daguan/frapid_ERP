using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.NPoco;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Menus
    {
        public static IEnumerable<MenuItemView> GetMenuItems(string menuName)
        {
            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<MenuItemView>(sql => sql.Where(c => c.MenuName == menuName)).OrderBy(c => c.Sort);
            }
        }
    }
}