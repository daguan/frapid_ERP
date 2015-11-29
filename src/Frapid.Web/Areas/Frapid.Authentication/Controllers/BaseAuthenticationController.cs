using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Authentication.DTO;

namespace Frapid.Authentication.Controllers
{
    public class BaseAuthenticationController : FrapidController
    {
        protected ActionResult OnAuthenticated(SignInResult result)
        {
            if (!result.Status) return Json(result);

            HttpCookie cookie = AuthenticationManager.GetCookie(result.SignInId.ToString(CultureInfo.InvariantCulture));
            Response.Cookies.Add(cookie);

            return Json(result);
        }
    }
}