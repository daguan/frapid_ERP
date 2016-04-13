using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Menus
    {
        public static IEnumerable<MenuItemView> GetMenuItems(string tenant, string menuName)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<MenuItemView>(sql => sql.Where(c => c.MenuName == menuName)).OrderBy(c => c.Sort);
            }
        }
    }
}