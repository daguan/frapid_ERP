using System;
using Frapid.i18n;

namespace Frapid.Authentication.ViewModels
{
    public class SignIn
    {
        public string Email { get; set; }
        public string Challenge { get; private set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Culture { get; set; }
        public bool AllowRegistration { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookScope { get; set; }
        public string GoogleSigninClientId { get; set; }
        public string GoogleSignInScope { get; set; }

        public SignIn()
        {
            this.Challenge = Guid.NewGuid().ToString();
            this.Culture = CultureManager.GetCurrent().Name;
        }
    }
}