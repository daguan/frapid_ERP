using Frapid.ApplicationState.Cache;
using Frapid.Authentication.Models;
using Frapid.DataAccess;

namespace Frapid.Authentication.DAL
{
    public static class FacebookSignIn
    {
        public static FacebookSignInResult SignIn(long facebookUserId, string token, string browser, string ipAddress,
            string culture)
        {
            const string sql = "SELECT * FROM auth.fb_sign_in(@0, @1, @2, @3, @4);";
            return Factory.Single<FacebookSignInResult>(AppUsers.GetCatalog(), sql, facebookUserId, token, browser,
                ipAddress, culture);
        }
    }
}