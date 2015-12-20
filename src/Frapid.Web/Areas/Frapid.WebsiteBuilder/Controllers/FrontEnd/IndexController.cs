using System;
using System.Web.Mvc;
using AutoMapper;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class IndexController : WebsiteBuilderController
    {
        private const string TokenKey = "Token";

        [Route("")]
        [Route("site/{categoryAlias}/{alias}")]
        public ActionResult Index(string categoryAlias = "", string alias = "")
        {
            string token = Guid.NewGuid().ToString();
            this.Session[TokenKey] = token;
            this.ViewBag.Token = token;

            var content = DAL.Contents.GetPublished(categoryAlias, alias);
            Mapper.CreateMap<PublishedContentView, ViewModels.Content>();
            var model = Mapper.Map<ViewModels.Content>(content);

            bool isHomepage = string.IsNullOrWhiteSpace(categoryAlias) && string.IsNullOrWhiteSpace(alias);

            string path = GetLayoutPath();
            string layout = isHomepage ? this.GetHomepageLayout() : this.GetLayout();

            if (model == null)
            {
                return this.View(GetLayoutPath() + "404.cshtml");
            }

            model.LayoutPath = path;
            model.Layout = layout;

            return this.View(this.GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
        }
    }
}