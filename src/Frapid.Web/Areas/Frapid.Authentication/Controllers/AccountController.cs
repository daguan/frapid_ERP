using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Frapid.Authentication.DAL;
using Frapid.Authentication.DTO;
using Frapid.Authentication.ViewModels;

namespace Frapid.Authentication.Controllers
{
    public class AccountController : BaseAuthenticationController
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

            ConfigurationProfile profile = Configuration.GetActiveProfile();
            Mapper.CreateMap<ConfigurationProfile, SignIn>();
            SignIn model = Mapper.Map<SignIn>(profile);

            return View("~/Areas/Frapid.Authentication/Views/Account/SignIn.cshtml", model);
        }
    }
}