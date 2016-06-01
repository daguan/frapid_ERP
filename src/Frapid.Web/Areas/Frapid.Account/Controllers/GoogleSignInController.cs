using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.InputModels;
using Frapid.Account.RemoteAuthentication;
using Frapid.Areas.CSRF;
using Npgsql;

namespace Frapid.Account.Controllers
{
    [AntiForgery]
    public class GoogleSignInController: BaseAuthenticationController
    {
        [Route("account/google/sign-in")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GoogleSignInAsync(GoogleAccount account)
        {
            try
            {
                var oauth = new GoogleAuthentication();
                var result = await oauth.AuthenticateAsync(account, this.RemoteUser);
                return await this.OnAuthenticatedAsync(result);
            }
            catch(NpgsqlException)
            {
                return this.AccessDenied();
            }
        }
    }
}