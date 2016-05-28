using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Authorization.DTO;
using Frapid.Authorization.ViewModels;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.NPoco;
using GroupEntityAccessPolicy = Frapid.Authorization.DTO.GroupEntityAccessPolicy;

namespace Frapid.Authorization.DAL
{
    public static class AccessPolicy
    {
        public static async Task<IEnumerable<GroupEntityAccessPolicy>> GetGroupPolicyAsync(string tenant, int officeId, int roleId)
        {
            using(var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<GroupEntityAccessPolicy>().Where(x => x.OfficeId.Equals(officeId) && x.RoleId.Equals(roleId)).ToListAsync();
            }
        }

        public static async Task SaveGroupPolicyAsync(string tenant, int officeId, int roleId, List<AccessPolicyInfo> policies)
        {
            using(var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                db.BeginTransaction();

                var sql = new Sql();
                sql.Append("DELETE FROM auth.group_entity_access_policy");
                sql.Append("WHERE office_id = @0", officeId);
                sql.Append("AND role_id = @0", roleId);

                await db.ExecuteAsync(sql);


                foreach(var policy in policies)
                {
                    var poco = new GroupEntityAccessPolicy
                               {
                                   EntityName = policy.EntityName,
                                   OfficeId = officeId,
                                   RoleId = roleId,
                                   AccessTypeId = policy.AccessTypeId,
                                   AllowAccess = policy.AllowAccess
                               };


                    await db.InsertAsync("auth.group_entity_access_policy", "group_entity_access_policy_id", true, poco);
                }

                db.CompleteTransaction();
            }
        }

        public static async Task<IEnumerable<EntityAccessPolicy>> GetPolicyAsync(string tenant, int officeId, int userId)
        {
            using(var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<EntityAccessPolicy>().Where(x => x.OfficeId.Equals(officeId) && x.UserId.Equals(userId)).ToListAsync();
            }
        }

        public static async Task SavePolicyAsync(string tenant, int officeId, int userId, List<AccessPolicyInfo> policies)
        {
            using(var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                db.BeginTransaction();

                var sql = new Sql();
                sql.Append("DELETE FROM auth.entity_access_policy");
                sql.Append("WHERE office_id = @0", officeId);
                sql.Append("AND user_id = @0", userId);

                await db.ExecuteAsync(sql);


                foreach(var policy in policies)
                {
                    var poco = new EntityAccessPolicy
                               {
                                   EntityName = policy.EntityName,
                                   OfficeId = officeId,
                                   UserId = userId,
                                   AccessTypeId = policy.AccessTypeId,
                                   AllowAccess = policy.AllowAccess
                               };


                    await db.InsertAsync("auth.entity_access_policy", "entity_access_policy_id", true, poco);
                }

                db.CompleteTransaction();
            }
        }
    }
}