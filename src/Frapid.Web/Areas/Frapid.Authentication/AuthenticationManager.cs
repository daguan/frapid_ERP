using System;
using System.Web;
using System.Web.Security;

namespace Frapid.Authentication
{
    public class AuthenticationManager
    {        
        public static HttpCookie GetCookie(string signInId)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, signInId, DateTime.Now, DateTime.Now.AddMinutes(30), true, string.Empty, FormsAuthentication.FormsCookiePath);
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