using System.Web.Mvc;
using AutoMapper;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class IndexController : WebsiteBuilderController
    {
        [Route("")]
        [Route("site/{*alias}")]
        public ActionResult Index(string alias = "")
        {
            Content content = DAL.Content.GetPublished(alias);
            Mapper.CreateMap<Content, Models.Content>();
            Models.Content model = Mapper.Map<Models.Content>(content);

            string path = GetLayoutPath();
            string layout = "Layout.cshtml";

            if (model == null)
            {
                return View(GetRazorView<AreaRegistration>("layouts/404.cshtml"),
                    new Models.Content {LayoutPath = path, Layout = layout});
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return View(GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
        }
    }
}