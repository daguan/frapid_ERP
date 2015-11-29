using System.Web.Mvc;
using Frapid.WebsiteBuilder.Controllers;

namespace Frapid.Authentication.Controllers
{
    public class ResetConfirmationController : WebsiteBuilderController
    {
        [Route("account/reset/confirm")]
        public ActionResult Index(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Redirect("/site/404");
            }

            DTO.Reset reset = DAL.Reset.GetIfActive(token);

            if (reset == null)
            {
                return Redirect("/site/404");
            }

            return View(GetRazorView<AreaRegistration>("ResetConfirmation/Index.cshtml"));
        }

        [Route("account/reset/confirm")]
        [HttpPost]
        public ActionResult Confirm()
        {
            var token = Request.QueryString["token"];
            var password = Request.QueryString["password"];

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password))
            {
                return Json(false);
            }

            DTO.Reset reset = DAL.Reset.GetIfActive(token);
            if (reset != null)
            {
                DAL.Reset.CompleteReset(token, password);
                return Json(true);
            }

            return Json(false);
        }
    }
}