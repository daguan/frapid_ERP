using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Authentication.DTO;
using Frapid.Authentication.Models;
using Frapid.Authentication.RemoteAuthentication;

namespace Frapid.Authentication.Controllers
{
    public class FacebookSignInController : BaseAuthenticationController
    {
        [Route("account/facebook/sign-in")]
        [HttpPost]
        public async Task<ActionResult> FacebookSignInAsync(FacebookSiginInDetail detail)
        {
            FacebookAuthentication auth = new FacebookAuthentication();
            SignInResult result =
                await auth.AuthenticateAsync(detail.FacebookUserId, detail.Email, detail.Token, GetRemoteUser());
            return OnAuthenticated(result);
        }
    }
}