using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Dashboard.DAL;

namespace Frapid.Dashboard.Controllers
{
    public class AppController : FrapidController
    {
        [Route("dashboard/my/apps")]
        [RestrictAnonymous]
        public ActionResult GetApps()
        {
            return Json(App.Get(), JsonRequestBehavior.AllowGet);
        }
    }
}