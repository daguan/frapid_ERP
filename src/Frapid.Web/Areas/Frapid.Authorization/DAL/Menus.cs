using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Authorization.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;

namespace Frapid.Authorization.DAL
{
    public static class Menus
    {
        public static async Task<IEnumerable<Menu>> GetMenusAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Menu>().OrderBy(x => x.Sort).ThenBy(x => x.MenuId).ToListAsync().ConfigureAwait(false);
            }
        }

        public static async Task<int[]> GetGroupPolicyAsync(string tenant, int officeId, int roleId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var awaiter = await db.Query<GroupMenuAccessPolicy>()
                    .Where(x => x.OfficeId.Equals(officeId) && x.RoleId.Equals(roleId))
                    .ToListAsync().ConfigureAwait(false);

                return awaiter.Select(x => x.MenuId)
                    .ToArray();
            }
        }

        public static async Task<IEnumerable<MenuAccessPolicy>> GetPolicyAsync(string tenant, int officeId, int userId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    await
                        db.Query<MenuAccessPolicy>()
                            .Where(x => x.OfficeId.Equals(officeId) && x.UserId.Equals(userId))
                            .ToListAsync();
            }
        }

        public static async Task SaveGroupPolicyAsync(string tenant, int officeId, int roleId, int[] menuIds)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "auth.save_group_menu_policy",
                new[] {"@0", "@1", "@2", "@3"});
            await
                Factory.NonQueryAsync(tenant, sql, roleId, officeId, string.Join(",", menuIds ?? new int[0]),
                    string.Empty);
        }

        public static async Task SavePolicyAsync(string tenant, int officeId, int userId, int[] allowed, int[] disallowed)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "auth.save_user_menu_policy",
                new[] {"@0", "@1", "@2", "@3"});
            await
                Factory.NonQueryAsync(tenant, sql, userId, officeId, string.Join(",", allowed ?? new int[0]),
                    string.Join(",", disallowed ?? new int[0]));
        }
    }
}