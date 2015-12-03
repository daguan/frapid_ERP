using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.DAL;

namespace Frapid.Dashboard.Controllers
{
    public class AppController : FrapidController
    {
        [Route("dashboard/my/apps")]
        [Authorize]
        public ActionResult GetApps()
        {
            return Json(App.Get(), JsonRequestBehavior.AllowGet);
        }
    }
}