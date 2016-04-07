using System;
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

            var offices = Offices.GetOffices();
            var users = Users.GetUsers();
            var menus = Menus.GetMenus();
            return new UserMenuPolicy
            {
                Menus = menus,
                Offices = offices,
                Users = users
            };
        }

        public static void Save(UserMenuPolicyInfo model)
        {
            Menus.SavePolicy(model.OfficeId, model.UserId, model.Allowed, model.Disallowed);
        }


        internal static IEnumerable<MenuAccessPolicy> Get(int officeId, int userId)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new List<MenuAccessPolicy>();
            }

            return Menus.GetPolicy(officeId, userId);
        }
    }
}