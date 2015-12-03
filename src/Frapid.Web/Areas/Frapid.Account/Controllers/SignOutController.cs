using System.Web.Mvc;
using System.Web.Security;

namespace Frapid.Account.Controllers
{
    public class SignOutController : BaseAuthenticationController
    {
        [Route("account/sign-out")]
        [Route("account/log-out")]
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/account/sign-in");
        }
    }
}