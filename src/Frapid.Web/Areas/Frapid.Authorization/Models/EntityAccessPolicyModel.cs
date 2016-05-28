using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;
using Mapster;

namespace Frapid.Authorization.Models
{
    public static class EntityAccessPolicyModel
    {
        public static async Task<UserEntityAccessPolicy> GetAsync()
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new UserEntityAccessPolicy();
            }

            string tenant = AppUsers.GetTenant();
            var offices = await Offices.GetOfficesAsync(tenant);
            var users = await Users.GetUsersAsync(tenant);


            return new UserEntityAccessPolicy
            {
                Offices = offices,
                Users = users,
                AccessTypes = AccessType.GetAccessTypes(),
                Entities = await Entity.GetEntitiesAsync()
            };
        }

        internal static async Task<List<AccessPolicyInfo>> GetAsync(int officeId, int userId)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return new List<AccessPolicyInfo>();
            }

            string tenant = AppUsers.GetTenant();
            var data = await AccessPolicy.GetPolicyAsync(tenant, officeId, userId);
            return data.Adapt<List<AccessPolicyInfo>>();
        }

        public static async Task SaveAsync(int officeId, int userId, List<AccessPolicyInfo> model)
        {
            if (!(await AppUsers.GetCurrentAsync()).IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            await AccessPolicy.SavePolicyAsync(tenant, officeId, userId, model);
        }
    }
}