using Frapid.ApplicationState.Cache;
using Frapid.Authentication.DTO;
using Frapid.DataAccess;

namespace Frapid.Authentication.DAL
{
    public static class FacebookSignIn
    {
        public static SignInResult SignIn(string facebookUserId, string email, string name, string token, string browser,
            string ipAddress, string culture)
        {
            const string sql =
                "SELECT * FROM auth.fb_sign_in(@0::text, @1::text, @2::text, @3::text, @4::text, @5::text, @6::text);";
            return Factory.Single<SignInResult>(AppUsers.GetCatalog(), sql, facebookUserId, email, name, token, browser,
                ipAddress, culture);
        }
    }
}