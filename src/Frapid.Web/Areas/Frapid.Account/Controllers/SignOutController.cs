using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Frapid.Account.DAL;

namespace Frapid.Account.Controllers
{
    public class SignOutController : BaseAuthenticationController
    {
        [Route("account/sign-out")]
        [Route("account/log-out")]
        public async Task<ActionResult> SignOutAsync()
        {
            if (!string.IsNullOrWhiteSpace(this.AppUser?.ClientToken))
            {
                await AccessTokens.RevokeAsync(this.Tenant, this.AppUser.ClientToken).ConfigureAwait(true);
            }

            FormsAuthentication.SignOut();
            return View(GetRazorView<AreaRegistration>("SignOut/Index.cshtml", this.Tenant));
        }
    }
}