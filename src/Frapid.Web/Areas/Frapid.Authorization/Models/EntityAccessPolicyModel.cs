using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;
using Frapid.Authorization.ViewModels;
using Mapster;

namespace Frapid.Authorization.Models
{
    public static class EntityAccessPolicyModel
    {
        public static UserEntityAccessPolicy Get()
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new UserEntityAccessPolicy();
            }

            string tenant = AppUsers.GetTenant();
            var offices = Offices.GetOffices(tenant);
            var users = Users.GetUsers(tenant);


            return new UserEntityAccessPolicy
            {
                Offices = offices,
                Users = users,
                AccessTypes = AccessType.GetAccessTypes(),
                Entities = Entity.GetEntities()
            };
        }

        internal static List<AccessPolicyInfo> Get(int officeId, int userId)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return new List<AccessPolicyInfo>();
            }

            string tenant = AppUsers.GetTenant();
            var data = AccessPolicy.GetPolicy(tenant, officeId, userId);
            return data.Adapt<List<AccessPolicyInfo>>();
        }

        public static void Save(int officeId, int userId, List<AccessPolicyInfo> model)
        {
            if (!AppUsers.GetCurrent().IsAdministrator)
            {
                return;
            }

            string tenant = AppUsers.GetTenant();
            AccessPolicy.SavePolicy(tenant, officeId, userId, model);
        }
    }
}