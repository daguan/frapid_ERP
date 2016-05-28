using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;
using Mapster;

namespace Frapid.Authorization.Models
{
    public static class GroupEntityAccessPolicyModel
    {
        public static async Task<GroupEntityAccessPolicy> GetAsync()
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new GroupEntityAccessPolicy();
            }

            string tenant = AppUsers.GetTenant();

            var offices = await Offices.GetOfficesAsync(tenant);
            var roles = await Roles.GetRolesAsync(tenant);

            return new GroupEntityAccessPolicy
            {
                Offices = offices,
                Roles = roles,
                AccessTypes = AccessType.GetAccessTypes(),
                Entities = await Entity.GetEntitiesAsync()
            };
        }

        internal static async Task<List<AccessPolicyInfo>> GetAsync(int officeId, int roleId)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new List<AccessPolicyInfo>();
            }

            string tenant = AppUsers.GetTenant();
            var data = await AccessPolicy.GetGroupPolicyAsync(tenant, officeId, roleId);
            return data.Adapt<List<AccessPolicyInfo>>();
        }

        public static async Task SaveAsync(int officeId, int roleId, List<AccessPolicyInfo> model)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            await AccessPolicy.SaveGroupPolicyAsync(tenant, officeId, roleId, model);
        }
    }
}