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
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new UserEntityAccessPolicy();
            }

            string tenant = AppUsers.GetTenant();
            var offices = await Offices.GetOfficesAsync(tenant).ConfigureAwait(false);
            var users = await Users.GetUsersAsync(tenant).ConfigureAwait(false);


            return new UserEntityAccessPolicy
            {
                Offices = offices,
                Users = users,
                AccessTypes = AccessType.GetAccessTypes(),
                Entities = await Entity.GetEntitiesAsync().ConfigureAwait(false)
            };
        }

        internal static async Task<List<AccessPolicyInfo>> GetAsync(int officeId, int userId)
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return new List<AccessPolicyInfo>();
            }

            string tenant = AppUsers.GetTenant();
            var data = await AccessPolicy.GetPolicyAsync(tenant, officeId, userId).ConfigureAwait(false);
            return data.Adapt<List<AccessPolicyInfo>>();
        }

        public static async Task SaveAsync(int officeId, int userId, List<AccessPolicyInfo> model)
        {
            if (!(await AppUsers.GetCurrentAsync().ConfigureAwait(false)).IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            await AccessPolicy.SavePolicyAsync(tenant, officeId, userId, model).ConfigureAwait(false);
        }
    }
}