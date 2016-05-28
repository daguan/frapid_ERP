using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Menus
    {
        public static async Task<IEnumerable<MenuItemView>> GetMenuItemsAsync(string tenant, string menuName)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<MenuItemView>().Where(c => c.MenuName == menuName).OrderBy(c => c.Sort).ToListAsync();
            }
        }
    }
}