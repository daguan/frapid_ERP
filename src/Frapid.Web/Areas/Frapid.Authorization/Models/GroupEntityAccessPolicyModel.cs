using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;
using Mapster;

namespace Frapid.Authorization.Models
{
    public static class GroupEntityAccessPolicyModel
    {
        public static GroupEntityAccessPolicy Get()
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new GroupEntityAccessPolicy();
            }

            var offices = Offices.GetOffices();
            var roles = Roles.GetRoles();

            return new GroupEntityAccessPolicy
            {
                Offices = offices,
                Roles = roles,
                AccessTypes = AccessType.GetAccessTypes(),
                Entities = Entity.GetEntities()
            };
        }

        internal static List<AccessPolicyInfo> Get(int officeId, int roleId)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new List<AccessPolicyInfo>();
            }

            var data = AccessPolicy.GetGroupPolicy(officeId, roleId);
            return data.Adapt<List<AccessPolicyInfo>>();
        }


        public static void Save(int officeId, int roleId, List<AccessPolicyInfo> model)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return;
            }

            AccessPolicy.SaveGroupPolicy(officeId, roleId, model);
        }
    }
}