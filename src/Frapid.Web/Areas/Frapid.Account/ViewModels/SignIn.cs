using System;
using System.Web;
using Frapid.i18n;

namespace Frapid.Account.ViewModels
{
    public class SignIn
    {
        public SignIn()
        {
            Challenge = Guid.NewGuid().ToString();
            Culture = CultureManager.GetCurrent().Name;
            HttpContext.Current.Session["Challenge"] = Challenge;
        }

        public string Email { get; set; }
        public string Challenge { get; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Culture { get; set; }
        public bool AllowRegistration { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookScope { get; set; }
        public string GoogleSigninClientId { get; set; }
        public string GoogleSignInScope { get; set; }
    }
}