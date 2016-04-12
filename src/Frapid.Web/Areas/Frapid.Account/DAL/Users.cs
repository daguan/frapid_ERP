using System;
using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Configuration;

namespace Frapid.Account.DAL
{
    public static class Users
    {
        public static User Get(string email)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(u => u.Email == email)).FirstOrDefault();
            }
        }

        public static void ChangePassword(int userId, string newPassword, RemoteUser remoteUser)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                var user = db.FetchBy<User>(sql => sql.Where(u => u.UserId == userId)).FirstOrDefault();

                if (user != null)
                {
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
}