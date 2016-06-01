using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.Models
{
    public static class MenuService
    {
        public static IEnumerable<MenuItemView> GetMenus(string menuName)
        {
            string tenant = AppUsers.GetTenant();
            var task = Task.Run(async () => await Menus.GetMenuItemsAsync(tenant, menuName));

            return task.Result;
        }
    }
}