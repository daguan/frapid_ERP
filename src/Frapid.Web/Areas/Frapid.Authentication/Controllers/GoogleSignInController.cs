using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Authentication.DTO;
using Frapid.Authentication.Models;
using Frapid.Authentication.RemoteAuthentication;

namespace Frapid.Authentication.Controllers
{
    public class GoogleSignInController : BaseAuthenticationController
    {
        [Route("account/google/sign-in")]
        [HttpPost]
        public async Task<ActionResult> GoogleSignInAsync(GoogleSignInDetail detail)
        {
            GoogleAuthentication oauth = new GoogleAuthentication();
            SignInResult result = await oauth.AuthenticateAsync(detail.Email, detail.Name, detail.Token, GetRemoteUser());

            return this.OnAuthenticated(result);
        }
    }
}