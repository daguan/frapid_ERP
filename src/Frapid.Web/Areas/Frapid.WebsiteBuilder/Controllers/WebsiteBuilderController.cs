using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class WebsiteBuilderController : FrapidController
    {
        public WebsiteBuilderController()
        {
            ViewBag.LayoutPath = GetLayoutPath();
            ViewBag.Layout = "Layout.cshtml";
        }

        protected string GetLayoutPath()
        {
            string layout = "~/Catalogs/{0}/Areas/Frapid.WebsiteBuilder/Views/Layouts/";
            string catalog = AppUsers.GetCatalog();
            layout = string.Format(CultureInfo.InvariantCulture, layout, catalog);

            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return "~/Views/Site/Layouts/";
        }
    }
}