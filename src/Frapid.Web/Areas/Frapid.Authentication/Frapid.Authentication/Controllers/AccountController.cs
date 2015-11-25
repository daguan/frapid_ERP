using System.Web.Mvc;

namespace Frapid.Authentication.Controllers
{
    public class AccountController : Controller
    {
        [Route("account/{*path}")]
        public ActionResult Main(string path)
        {
            return Content("Foo " + path);
        }

        [Route("account/sign-in")]
        [Route("account/log-in")]
        public ActionResult SignIn()
        {
            return View("~/Areas/Frapid.Authentication/Frapid.Authentication/Views/Account/SignIn.cshtml");
        }
    }
}