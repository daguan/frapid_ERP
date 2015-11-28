using System.Globalization;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.i18n;
using WebsiteBuilder.Models;

namespace WebsiteBuilder.Controllers
{
    public class WebsiteBuilderController : Controller
    {
        public WebsiteBuilderController()
        {
            ViewBag.LayoutPath = this.GetLayoutPath();
        }

        protected string GetLayoutPath()
        {
            var layout = "~/Catalogs/{0}/Areas/WebsiteBuilder/layouts/";
            var catalog = AppUsers.GetCatalog();
            return string.Format(CultureInfo.InvariantCulture, layout, catalog);
        }

        protected RemoteUser GetRemoteUser()
        {
            return new RemoteUser
            {
                Browser = Request.Browser.Browser,
                IpAddress = Request.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name
            };
        }
    }
}