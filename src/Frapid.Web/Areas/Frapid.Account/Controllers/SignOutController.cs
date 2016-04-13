using System.Web.Mvc;
using System.Web.Security;
using Frapid.Account.DAL;
using Frapid.ApplicationState.Cache;
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
            string tenant = AppUsers.GetTenant();

            if (this.MetaUser != null)
            {
                AccessTokens.Revoke(tenant, this.MetaUser.ClientToken);
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