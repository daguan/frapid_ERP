using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;

namespace Frapid.Authorization.Models
{
    public static class GroupMenuPolicyModel
    {
        public static async Task<GroupMenuPolicy> GetAsync()
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new GroupMenuPolicy();
            }

            string tenant = AppUsers.GetTenant();

            var offices = await Offices.GetOfficesAsync(tenant).ConfigureAwait(false);
            var roles = await Roles.GetRolesAsync(tenant).ConfigureAwait(false);
            var menus = await Menus.GetMenusAsync(tenant).ConfigureAwait(false);

            return new GroupMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Roles = roles
            };
        }

        internal static async Task<GroupMenuPolicyInfo> GetAsync(int officeId, int roleId)
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new GroupMenuPolicyInfo();
            }

            string tenant = AppUsers.GetTenant();
            var menuIds = await Menus.GetGroupPolicyAsync(tenant, officeId, roleId).ConfigureAwait(false);

            return new GroupMenuPolicyInfo
            {
                RoleId = roleId,
                OfficeId = officeId,
                MenuIds = menuIds
            };
        }

        public static async Task SaveAsync(GroupMenuPolicyInfo model)
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            await Menus.SaveGroupPolicyAsync(tenant, model.OfficeId, model.RoleId, model.MenuIds).ConfigureAwait(false);
        }
    }
}