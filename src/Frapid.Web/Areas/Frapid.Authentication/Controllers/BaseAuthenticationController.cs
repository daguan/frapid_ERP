using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Frapid.Authentication.DTO;
using Frapid.i18n;
using WebsiteBuilder.Models;

namespace Frapid.Authentication.Controllers
{
    public class BaseAuthenticationController : Controller
    {
        protected ActionResult OnAuthenticated(SignInResult result)
        {
            if (!result.Status) return Json(result);

            HttpCookie cookie = AuthenticationManager.GetCookie(result.SignInId.ToString(CultureInfo.InvariantCulture));
            Response.Cookies.Add(cookie);

            return Json(result);
        }

        protected RemoteUser GetRemoteUser()
        {
            return new RemoteUser
            {
                Browser = Request.Browser.Browser,
                IpAddress = Request.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name
            };
        }
    }
}