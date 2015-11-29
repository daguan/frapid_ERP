using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Authentication.DTO;
using Frapid.DataAccess;

namespace Frapid.Authentication.DAL
{
    public static class GoogleSignIn
    {
        public static SignInResult SignIn(string email, string name, string token, string browser, string ipAddress,
            string culture)
        {
            const string sql = "SELECT * FROM auth.google_sign_in(@0::text, @1::text, @2::text, @3::text, @4::text, @5::text);";
            return Factory.Get<SignInResult>(AppUsers.GetCatalog(), sql, email, name, token, browser,
                ipAddress, culture).FirstOrDefault();
        }
    }
}