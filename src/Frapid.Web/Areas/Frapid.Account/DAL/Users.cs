using System;
using System.Linq;
using Frapid.Account.DTO;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.Account.DAL
{
    public static class Users
    {
        public static User Get(string tenant, string email)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(u => u.Email == email)).FirstOrDefault();
            }
        }

        public static void ChangePassword(string tenant, int userId, string newPassword, RemoteUser remoteUser)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var user = db.FetchBy<User>(sql => sql.Where(u => u.UserId == userId)).FirstOrDefault();

                if (user == null)
                {
                    return;
                }

                user.Password = newPassword;
                user.AuditUserId = userId;
                user.AuditTs = DateTimeOffset.UtcNow;
                user.LastBrowser = remoteUser.Browser;
                user.LastIp = remoteUser.IpAddress;
                user.LastSeenOn = DateTimeOffset.UtcNow;

                db.Update("account.users", "user_id", user, userId);
            }
        }
    }
}