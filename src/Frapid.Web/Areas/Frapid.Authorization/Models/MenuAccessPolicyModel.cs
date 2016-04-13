using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.DTO;
using Frapid.Authorization.ViewModels;

namespace Frapid.Authorization.Models
{
    public static class MenuAccessPolicyModel
    {
        public static UserMenuPolicy Get()
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new UserMenuPolicy();
            }

            string tenant = AppUsers.GetTenant();
            var offices = Offices.GetOffices(tenant);
            var users = Users.GetUsers(tenant);
            var menus = Menus.GetMenus(tenant);
            return new UserMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Users = users
            };
        }

        public static void Save(UserMenuPolicyInfo model)
        {
            string tenant = AppUsers.GetTenant();
            Menus.SavePolicy(tenant, model.OfficeId, model.UserId, model.Allowed, model.Disallowed);
        }


        internal static IEnumerable<MenuAccessPolicy> Get(int officeId, int userId)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new List<MenuAccessPolicy>();
            }

            string tenant = AppUsers.GetTenant();
            return Menus.GetPolicy(tenant, officeId, userId);
        }
    }
}