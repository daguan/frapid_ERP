using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.DTO;
using Frapid.Account.InputModels;
using Frapid.Account.ViewModels;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Mapster;
using Npgsql;
using SignIn = Frapid.Account.ViewModels.SignIn;

namespace Frapid.Account.Controllers
{
    [AntiForgery]
    public class SignInController : BaseAuthenticationController
    {
        [Route("account/sign-in")]
        [Route("account/sign-in/social")]
        [Route("account/log-in")]
        [Route("account/log-in/social")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/dashboard");
            }

            var profile = ConfigurationProfiles.GetActiveProfile();


            var model = profile.Adapt<SignIn>() ?? new SignIn();
            return View(GetRazorView<AreaRegistration>("SignIn/Index.cshtml"), model);
        }

        [Route("account/sign-in")]
        [Route("account/log-in")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Do(SignInInfo model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            try
            {
                bool isValid = this.CheckPassword(model.Email, model.Password);

                if (!isValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var result = DAL.SignIn.Do(model.Email, model.OfficeId, this.RemoteUser.Browser,
                    this.RemoteUser.IpAddress, model.Culture.Or("en-US"));
                return this.OnAuthenticated(result, model);
            }
            catch (NpgsqlException)
            {
                return this.Failed("Access is denied.", HttpStatusCode.Forbidden);
            }
        }

        [Route("account/sign-in/offices")]
        [Route("account/log-in/offices")]
        [AllowAnonymous]
        public ActionResult GetOffices()
        {
            return this.Ok(Offices.GetOffices());
        }

        [Route("account/sign-in/languages")]
        [Route("account/log-in/languages")]
        [AllowAnonymous]
        public ActionResult GetLanguages()
        {
            var cultures =
                ConfigurationManager.GetConfigurationValue("ParameterConfigFileLocation", "Cultures").Split(',');
            var languages = (from culture in cultures
                select culture.Trim()
                into cultureName
                from info in
                    CultureInfo.GetCultures(CultureTypes.AllCultures)
                        .Where(x => x.TwoLetterISOLanguageName.Equals(cultureName))
                select new Language
                {
                    CultureCode = info.Name,
                    NativeName = info.NativeName
                }).ToList();

            return this.Ok(languages);
        }
    }
}