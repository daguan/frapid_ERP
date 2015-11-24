using System.Web.Mvc;

namespace Frapid.Web.Controllers
{
    public class DashboardController : Controller
    {
        [Route("dashboard")]
        public ActionResult Index()
        {
            return View();
        }
    }
}