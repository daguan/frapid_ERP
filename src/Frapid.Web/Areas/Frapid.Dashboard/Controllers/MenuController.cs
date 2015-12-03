using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.DAL;

namespace Frapid.Dashboard.Controllers
{
    public class MenuController : FrapidController
    {
        [Route("dashboard/my/menus")]
        [Authorize]
        public ActionResult GetMenus()
        {
            return Json(Menu.Get(), JsonRequestBehavior.AllowGet);
        }
    }
}