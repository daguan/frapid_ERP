using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AutoMapper;
using Frapid.Account.DTO;
using Frapid.Account.InputModels;
using Frapid.Account.ViewModels;
using Frapid.Configuration;
using Npgsql;
using Office = Frapid.Account.DAL.Office;

namespace Frapid.Account.Controllers
{
    public class SignInController : BaseAuthenticationController
    {
        [Route("account/sign-in")]
        [Route("account/log-in")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/dashboard");
            }

            var profile = DAL.Configuration.GetActiveProfile();
            Mapper.CreateMap<ConfigurationProfile, SignIn>();
            var model = Mapper.Map<SignIn>(profile);

            return View(GetRazorView<AreaRegistration>("SignIn/Index.cshtml"), model);
        }

        [Route("account/sign-in")]
        [Route("account/log-in")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Do(SignInInfo model)
        {
            Thread.Sleep(1000);

            string challenge = Session["Challenge"].ToString();
            if (model.Challenge != challenge)
            {
                return Redirect("/");
            }

            model.Browser = this.RemoteUser.Browser;
            model.IpAddress = this.RemoteUser.IpAddress;

            try
            {
                var result = DAL.SignIn.Do(model.Email, model.OfficeId, model.Challenge, model.Password,
                    model.Browser, model.IpAddress, model.Culture);
                return this.OnAuthenticated(result);
            }
            catch (NpgsqlException)
            {
                return Json("Access is denied.");
            }
        }

        [Route("account/sign-in/offices")]
        [Route("account/log-in/offices")]
        [AllowAnonymous]
        public ActionResult GetOffices()
        {
            return Json(Office.GetOffices(), JsonRequestBehavior.AllowGet);
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

            return Json(languages, JsonRequestBehavior.AllowGet);
        }
    }
}