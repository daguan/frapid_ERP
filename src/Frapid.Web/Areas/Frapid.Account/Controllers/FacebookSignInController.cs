using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Account.DTO;
using Frapid.Account.InputModels;
using Frapid.Account.RemoteAuthentication;
using Npgsql;

namespace Frapid.Account.Controllers
{
    public class FacebookSignInController : BaseAuthenticationController
    {
        [Route("account/facebook/sign-in")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> FacebookSignInAsync(FacebookAccount account)
        {
            FacebookAuthentication auth = new FacebookAuthentication();
            try
            {
                LoginResult result =
                    await auth.AuthenticateAsync(account, this.RemoteUser);
                return OnAuthenticated(result);
            }
            catch (NpgsqlException)
            {
                return Json("Access is denied.");
            }
        }
    }
}