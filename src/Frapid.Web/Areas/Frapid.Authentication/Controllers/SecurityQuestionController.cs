using System.Web.Mvc;

namespace Frapid.Authentication.Controllers
{
    public class SecurityQuestionController:Controller
    {
        [Route("account/security-questions")]
        public ActionResult Index()
        {
            return View();
        }
    }
}