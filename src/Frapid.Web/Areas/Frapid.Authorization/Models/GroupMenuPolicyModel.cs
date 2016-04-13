using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;

namespace Frapid.Authorization.Models
{
    public static class GroupMenuPolicyModel
    {
        public static GroupMenuPolicy Get()
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new GroupMenuPolicy();
            }

            string tenant = AppUsers.GetTenant();

            var offices = Offices.GetOffices(tenant);
            var roles = Roles.GetRoles(tenant);
            var menus = Menus.GetMenus(tenant);

            return new GroupMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Roles = roles
            };
        }

        internal static GroupMenuPolicyInfo Get(int officeId, int roleId)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new GroupMenuPolicyInfo();
            }

            string tenant = AppUsers.GetTenant();
            var menuIds = Menus.GetGroupPolicy(tenant, officeId, roleId);
            return new GroupMenuPolicyInfo
            {
                RoleId = roleId,
                OfficeId = officeId,
                MenuIds = menuIds
            };
        }

        public static void Save(GroupMenuPolicyInfo model)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            Menus.SaveGroupPolicy(tenant, model.OfficeId, model.RoleId, model.MenuIds);
        }
    }
}