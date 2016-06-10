using System;
using System.Threading.Tasks;
using Frapid.Account.DTO;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.NPoco;

namespace Frapid.Account.DAL
{
    public static class Users
    {
        public static async Task<User> GetAsync(string tenant, string email)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<User>().Where(u => u.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }

        public static async Task ChangePasswordAsync(string tenant, int userId, string newPassword, RemoteUser remoteUser)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var sql = new Sql("UPDATE account.users SET");
                sql.Append("password=@0,", newPassword);
                sql.Append("audit_user_id=@0,", userId);
                sql.Append("audit_ts=@0,", DateTimeOffset.UtcNow);
                sql.Append("last_ip=@0,", remoteUser.IpAddress);
                sql.Append("last_seen_on=@0", DateTimeOffset.UtcNow);
                sql.Where("user_id=@0", userId);

                await db.ExecuteAsync(sql).ConfigureAwait(false);
            }
        }
    }
}