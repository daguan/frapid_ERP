using System.Web.Mvc;
using System.Web.Security;
using Frapid.Account.DAL;
using Frapid.Areas.Authorization;

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
            return Redirect("/account/sign-in");
        }
    }
}