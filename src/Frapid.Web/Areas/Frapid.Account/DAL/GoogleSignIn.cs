using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Account.DTO;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework.Extensions;

namespace Frapid.Account.DAL
{
    public static class GoogleSignIn
    {
        public static LoginResult SignIn(string email, int officeId, string name, string token, string browser, string ipAddress,
            string culture)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.google_sign_in", new[] { "@0", "@1", "@2", "@3", "@4", "@5", "@6" });
            return Factory.Get<LoginResult>(AppUsers.GetTenant(), sql, email, officeId, name, token, browser,
                ipAddress, culture.Or("en-US")).FirstOrDefault();
        }
    }
}