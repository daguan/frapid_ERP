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

            string tenant = AppUsers.GetTenant();

            var offices = Offices.GetOffices(tenant);
            var roles = Roles.GetRoles(tenant);

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

            string tenant = AppUsers.GetTenant();
            var data = AccessPolicy.GetGroupPolicy(tenant, officeId, roleId);
            return data.Adapt<List<AccessPolicyInfo>>();
        }


        public static void Save(int officeId, int roleId, List<AccessPolicyInfo> model)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            AccessPolicy.SaveGroupPolicy(tenant, officeId, roleId, model);
        }
    }
}