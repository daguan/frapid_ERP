using System.IO;
using System.Net;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    [AntiForgery]
    public class LayoutController : DashboardController
    {
        [Route("dashboard/website/layouts")]
        [RestrictAnonymous]
        public ActionResult Master()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Index.cshtml"));
        }

        [Route("dashboard/website/layouts/save")]
        [HttpPost]
        [RestrictAnonymous]
        public ActionResult SaveLayoutFile(string theme, string fileName, string contents)
        {
            string path = HostingEnvironment.MapPath(Configuration.GetThemeDirectory());
            path += Path.Combine(path, theme);

            if (!Directory.Exists(path))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            path += Path.Combine(path, fileName);

            if (!System.IO.File.Exists(path))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            System.IO.File.WriteAllText(path, contents, Encoding.UTF8);
            return this.Json("OK");
        }
    }
}