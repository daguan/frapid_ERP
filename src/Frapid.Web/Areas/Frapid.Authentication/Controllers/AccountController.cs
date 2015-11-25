using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using Frapid.Authentication.Models;
using Frapid.i18n;

namespace Frapid.Authentication.Controllers
{
    public class AccountController : Controller
    {
        [Route("account/sign-out")]
        [Route("account/log-out")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }

        [Route("account/sign-in")]
        [Route("account/log-in")]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/dashboard");
            }

            return View("~/Areas/Frapid.Authentication/Views/Account/SignIn.cshtml");
        }

        [Route("account/facebook/sign-in")]
        [HttpPost]
        public ActionResult FacebookSignIn(FacebookSiginInDetail detail)
        {
            System.Threading.Thread.Sleep(1000);
            string browser  = Request.Browser.Browser;
            string ipAddress = Request.UserHostAddress;

            string culture = CultureManager.GetCurrent().Name;
            var result = DAL.FacebookSignIn.SignIn(detail.FacebookUserId, detail.Token, browser, ipAddress, culture);

            if (result.Status)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, result.SignInId.ToString(CultureInfo.InvariantCulture), DateTime.Now, DateTime.Now.AddMinutes(30), true, string.Empty, FormsAuthentication.FormsCookiePath);
                string encrypted = FormsAuthentication.Encrypt(ticket);

                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                {
                    Domain = FormsAuthentication.CookieDomain,
                    Path = ticket.CookiePath
                };

                Response.Cookies.Add(cookie);
            }

            return Json(result);
        }

        [Route("account/sign-up")]
        public ActionResult SignUp()
        {
            return View("~/Areas/Frapid.Authentication/Views/Account/SignUp.cshtml");
        }
    }

    
}