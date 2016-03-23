using System;
using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.NPoco;

namespace Frapid.Account.DAL
{
    public static class Users
    {
        public static User Get(string email)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(u => u.Email == email)).FirstOrDefault();
            }
        }

        public static void ChangePassword(int userId, string newPassword, RemoteUser remoteUser)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                var user = db.FetchBy<User>(sql => sql.Where(u => u.UserId== userId)).FirstOrDefault();

                if (user != null)
                {
                    user.Password = newPassword;
                    user.AuditUserId = userId;
                    user.AuditTs = DateTime.UtcNow;
                    user.LastBrowser = remoteUser.Browser;
                    user.LastIp = remoteUser.IpAddress;
                    user.LastSeenOn = DateTime.UtcNow;

                    db.Update("account.users", "user_id", user, userId);
                }
            }

        }
    }
}