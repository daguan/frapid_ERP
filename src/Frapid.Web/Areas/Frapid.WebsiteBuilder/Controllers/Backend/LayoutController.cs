using System.IO;
using System.Net;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.Controllers;
using Frapid.WebsiteBuilder.Models;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    [AntiForgery]
    public class LayoutController : DashboardController
    {
        [Route("dashboard/website/layouts")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Master()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Index.cshtml"));
        }

        [Route("dashboard/website/layouts/save")]
        [HttpPost]
        [RestrictAnonymous]
        public ActionResult SaveLayoutFile(string theme, string fileName, string contents)
        {
            bool result = LayoutManagerModel.SaveLayoutFile(theme, fileName, contents);

            if (!result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.Json("OK");
        }
    }
}