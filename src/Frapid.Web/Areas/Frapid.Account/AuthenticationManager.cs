using System;
using System.Web;
using System.Web.Security;

namespace Frapid.Account
{
    public class AuthenticationManager
    {        
        public static HttpCookie GetCookie(string loginId)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginId, DateTime.Now, DateTime.Now.AddMinutes(30), true, string.Empty, FormsAuthentication.FormsCookiePath);
            string encrypted = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                Domain = FormsAuthentication.CookieDomain,
                Path = ticket.CookiePath
            };
            return cookie;
        }
    }
}