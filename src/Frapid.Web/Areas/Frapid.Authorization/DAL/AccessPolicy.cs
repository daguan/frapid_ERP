using System.Collections.Generic;
using System.Dynamic;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DTO;
using Frapid.Authorization.ViewModels;
using Frapid.Configuration;
using Frapid.NPoco;
using GroupEntityAccessPolicy = Frapid.Authorization.DTO.GroupEntityAccessPolicy;

namespace Frapid.Authorization.DAL
{
    public static class AccessPolicy
    {
        public static IEnumerable<GroupEntityAccessPolicy> GetGroupPolicy(int officeId, int roleId)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return
                    db.FetchBy<GroupEntityAccessPolicy>(
                        sql => sql.Where(x => x.OfficeId.Equals(officeId) && x.RoleId.Equals(roleId)));
            }
        }

        public static void SaveGroupPolicy(int officeId, int roleId, List<AccessPolicyInfo> policies)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                db.BeginTransaction();

                var sql = new Sql();
                sql.Append("DELETE FROM auth.group_entity_access_policy");
                sql.Append("WHERE office_id = @0", officeId);
                sql.Append("AND role_id = @0", roleId);

                db.Execute(sql);


                foreach (var policy in policies)
                {
                    dynamic poco = new ExpandoObject();
                    poco.entity_name = policy.EntityName;
                    poco.office_id = officeId;
                    poco.role_id = roleId;
                    poco.access_type_id = policy.AccessTypeId;
                    poco.allow_access = policy.AllowAccess;

                    db.Insert("auth.group_entity_access_policy", "group_entity_access_policy_id", true, poco);
                }

                db.CompleteTransaction();
            }
        }

        public static object GetPolicy(int officeId, int userId)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return
                    db.FetchBy<EntityAccessPolicy>(
                        sql => sql.Where(x => x.OfficeId.Equals(officeId) && x.UserId.Equals(userId)));
            }
        }

        public static void SavePolicy(int officeId, int userId, List<AccessPolicyInfo> policies)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                db.BeginTransaction();

                var sql = new Sql();
                sql.Append("DELETE FROM auth.entity_access_policy");
                sql.Append("WHERE office_id = @0", officeId);
                sql.Append("AND user_id = @0", userId);

                db.Execute(sql);


                foreach (var policy in policies)
                {
                    dynamic poco = new ExpandoObject();
                    poco.entity_name = policy.EntityName;
                    poco.office_id = officeId;
                    poco.user_id = userId;
                    poco.access_type_id = policy.AccessTypeId;
                    poco.allow_access = policy.AllowAccess;

                    db.Insert("auth.entity_access_policy", "entity_access_policy_id", true, poco);
                }

                db.CompleteTransaction();
            }
        }
    }
}