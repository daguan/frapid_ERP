using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.DTO;
using Frapid.Authorization.ViewModels;

namespace Frapid.Authorization.Models
{
    public static class MenuAccessPolicyModel
    {
        public static async Task<UserMenuPolicy> GetAsync()
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new UserMenuPolicy();
            }

            string tenant = AppUsers.GetTenant();
            var offices = await Offices.GetOfficesAsync(tenant).ConfigureAwait(false);
            var users = await Users.GetUsersAsync(tenant).ConfigureAwait(false);
            var menus = await Menus.GetMenusAsync(tenant).ConfigureAwait(false);

            return new UserMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Users = users
            };
        }

        public static async Task SaveAsync(UserMenuPolicyInfo model)
        {
            string tenant = AppUsers.GetTenant();
            await Menus.SavePolicyAsync(tenant, model.OfficeId, model.UserId, model.Allowed, model.Disallowed).ConfigureAwait(false);
        }

        internal static async Task<IEnumerable<MenuAccessPolicy>> GetAsync(int officeId, int userId)
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new List<MenuAccessPolicy>();
            }

            string tenant = AppUsers.GetTenant();
            return await Menus.GetPolicyAsync(tenant, officeId, userId).ConfigureAwait(false);
        }
    }
}