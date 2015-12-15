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
            Content content = DAL.Contents.GetPublished(alias);
            Mapper.CreateMap<Content, ViewModels.Content>();
            ViewModels.Content model = Mapper.Map<ViewModels.Content>(content);

            string path = GetLayoutPath();
            string layout = this.GetDefaultDocument();

            if (model == null)
            {
                return View(GetRazorView<AreaRegistration>("layouts/404.cshtml"),
                    new ViewModels.Content {LayoutPath = path, Layout = layout});
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return View(GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
        }
    }
}