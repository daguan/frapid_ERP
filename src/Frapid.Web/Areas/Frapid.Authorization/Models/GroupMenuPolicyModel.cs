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
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new GroupMenuPolicy();
            }

            string tenant = AppUsers.GetTenant();

            var offices = await Offices.GetOfficesAsync(tenant);
            var roles = await Roles.GetRolesAsync(tenant);
            var menus = await Menus.GetMenusAsync(tenant);

            return new GroupMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Roles = roles
            };
        }

        internal static async Task<GroupMenuPolicyInfo> GetAsync(int officeId, int roleId)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new GroupMenuPolicyInfo();
            }

            string tenant = AppUsers.GetTenant();
            var menuIds = await Menus.GetGroupPolicyAsync(tenant, officeId, roleId);

            return new GroupMenuPolicyInfo
            {
                RoleId = roleId,
                OfficeId = officeId,
                MenuIds = menuIds
            };
        }

        public static async Task SaveAsync(GroupMenuPolicyInfo model)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            await Menus.SaveGroupPolicyAsync(tenant, model.OfficeId, model.RoleId, model.MenuIds);
        }
    }
}