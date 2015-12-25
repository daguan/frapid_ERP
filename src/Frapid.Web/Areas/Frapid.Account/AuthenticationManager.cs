using System.Web;
using Frapid.Configuration;
using Frapid.TokenManager;

namespace Frapid.Account
{
    public class AuthenticationManager
    {
        public static HttpCookie GetCookie(Token token, string catalog, string loginId)
        {
            var cookie = new HttpCookie("MyCookieName")
            {
                Domain = DbConvention.GetDomain(),
                Name = "access_token",
                Value = token.ClientToken,
                HttpOnly = true,
                Secure = true
            };

            return cookie;
        }
    }
}