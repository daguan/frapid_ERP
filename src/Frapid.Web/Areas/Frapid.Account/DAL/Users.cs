using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;

namespace Frapid.Account.DAL
{
    public static class Users
    {
        public static User Get(string email)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<User>(sql => sql.Where(u => u.Email == email)).FirstOrDefault();
            }
        }
    }
}