using System.Web.Mvc;
using System.Web.Security;
using Frapid.Account.DAL;
using Frapid.Areas.Authorization;
using Frapid.Configuration;

namespace Frapid.Account.Controllers
{
    public class SignOutController : BaseAuthenticationController
    {
        [Route("account/sign-out")]
        [Route("account/log-out")]
        [RestrictAnonymous]
        public ActionResult SignOut()
        {
            if (this.MetaUser != null)
            {
                AccessTokens.Revoke(this.MetaUser.ClientToken);
            }

            FormsAuthentication.SignOut();
            return this.Redirect(this.GetReturnUrl());
        }

        private string GetReturnUrl()
        {
            string returnUrl = "/";
            var referrer = this.Request.UrlReferrer;
            if (referrer != null)
            {
                string domain = referrer.DnsSafeHost;
                bool wellKnown = DbConvention.IsValidDomain(domain);

                if (wellKnown)
                {
                    returnUrl = referrer.ToString();
                }
            }
            return returnUrl;
        }
    }
}