using System.Collections.Generic;
using System.Linq;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;

namespace Frapid.Authorization.DAL
{
    public static class Menus
    {
        public static IEnumerable<Menu> GetMenus(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Menu>(sql => sql).OrderBy(x => x.Sort).ThenBy(x => x.MenuId);
            }
        }

        public static int[] GetGroupPolicy(string tenant, int officeId, int roleId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<GroupMenuAccessPolicy>
                    (sql => sql.Where(x => x.OfficeId.Equals(officeId) && x.RoleId.Equals(roleId)))
                    .Select(x => x.MenuId)
                    .ToArray();
            }
        }

        public static IEnumerable<MenuAccessPolicy> GetPolicy(string tenant, int officeId, int userId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<MenuAccessPolicy>
                    (sql => sql.Where(x => x.OfficeId.Equals(officeId) && x.UserId.Equals(userId)));
            }
        }

        public static void SaveGroupPolicy(string tenant, int officeId, int roleId, int[] menuIds)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "auth.save_group_menu_policy", new[] {"@0", "@1", "@2", "@3"});
            Factory.NonQuery(tenant, sql, roleId, officeId, string.Join(",", menuIds ?? new int[0]), string.Empty);
        }

        public static void SavePolicy(string tenant, int officeId, int userId, int[] allowed, int[] disallowed)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "auth.save_user_menu_policy", new[] {"@0", "@1", "@2", "@3"});
            Factory.NonQuery(tenant, sql, userId, officeId, string.Join(",", allowed ?? new int[0]), string.Join(",", disallowed ?? new int[0]));
        }
    }
}