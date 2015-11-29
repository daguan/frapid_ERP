using System.Web.Mvc;
using Frapid.WebsiteBuilder.Models;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class IndexController : WebsiteBuilderController
    {
        [Route("")]
        [Route("site/{*alias}")]
        public ActionResult Index(string alias = "")
        {
            Content model = DAL.Content.Get(alias);
            string path = GetLayoutPath();
            string layout = "Layout.cshtml";

            if (model == null)
            {
                return View(GetRazorView<AreaRegistration>("layouts/404.cshtml"),
                    new Content {LayoutPath = path, Layout = layout});
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return View(GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
        }
    }
}