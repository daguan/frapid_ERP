using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;

namespace Frapid.Account.Controllers
{
    public class BaseAuthenticationController : FrapidController
    {
        protected ActionResult OnAuthenticated(LoginResult result)
        {
            if (!result.Status) return Json(result);
            var globalLoginId = AppUsers.GetMetaLoginId(AppUsers.GetCatalog(), result.LoginId);
            HttpCookie cookie = AuthenticationManager.GetCookie(globalLoginId.ToString(CultureInfo.InvariantCulture));
            Response.Cookies.Add(cookie);

            return Json(result);
        }
    }
}