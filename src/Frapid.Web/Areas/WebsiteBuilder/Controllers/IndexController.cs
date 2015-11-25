using System.Globalization;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using WebsiteBuilder.Models;

namespace WebsiteBuilder.Controllers
{
    public class IndexController : Controller
    {
        [Route("")]
        [Route("site/{*alias}")]
        public ActionResult Index(string alias = "")
        {
            Content model = DAL.Content.Get(alias);
            var path = this.GetPath();
            var layout = "layout.cshtml";

            if (model == null)
            {
                return View(path + "404.cshtml", new Content {LayoutPath = path, Layout = layout});
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return View("~/Areas/WebsiteBuilder/Views/Index/Index.cshtml", model);
        }

        private string GetPath()
        {
            var layout = "~/Catalogs/{0}/Areas/WebsiteBuilder/layouts/";
            var catalog = AppUsers.GetCatalog();
            return string.Format(CultureInfo.InvariantCulture, layout, catalog);
        }

    }
}