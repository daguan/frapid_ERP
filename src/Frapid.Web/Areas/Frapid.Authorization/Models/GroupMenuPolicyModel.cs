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

            var offices = Offices.GetOffices();
            var roles = Roles.GetRoles();
            var menus = Menus.GetMenus();
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

            var menuIds = Menus.GetGroupPolicy(officeId, roleId);
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

            Menus.SaveGroupPolicy(model.OfficeId, model.RoleId, model.MenuIds);
        }
    }
}