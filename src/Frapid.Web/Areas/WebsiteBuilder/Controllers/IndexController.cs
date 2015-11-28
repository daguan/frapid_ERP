using System.Web.Mvc;
using WebsiteBuilder.Models;

namespace WebsiteBuilder.Controllers
{
    public class IndexController : WebsiteBuilderController
    {
        [Route("")]
        [Route("site/{*alias}")]
        public ActionResult Index(string alias = "")
        {
            Content model = DAL.Content.Get(alias);
            var path = this.GetLayoutPath();
            var layout = "layout.cshtml";

            if (model == null)
            {
                return View(path + "404.cshtml", new Content {LayoutPath = path, Layout = layout});
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return View("~/Areas/WebsiteBuilder/Views/Index/Index.cshtml", model);
        }


    }
}