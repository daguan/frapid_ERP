using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Account.DTO;
using Frapid.DataAccess;
using Frapid.Framework.Extensions;

namespace Frapid.Account.DAL
{
    public static class GoogleSignIn
    {
        public static LoginResult SignIn(string email, int officeId, string name, string token, string browser, string ipAddress,
            string culture)
        {
            const string sql = "SELECT * FROM account.google_sign_in(@0::text, @1::integer, @2::text, @3::text, @4::text, @5::text, @6::text);";
            return Factory.Get<LoginResult>(AppUsers.GetTenant(), sql, email, officeId, name, token, browser,
                ipAddress, culture.Or("en-US")).FirstOrDefault();
        }
    }
}