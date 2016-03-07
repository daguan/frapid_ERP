using System.Linq;
using System.Web.Mvc;
using Frapid.Areas.Caching;
using Frapid.Configuration;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Models;
using Frapid.WebsiteBuilder.ViewModels;
using Npgsql;
using Serilog;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class BlogController : WebsiteBuilderController
    {
        [Route("blog/{categoryAlias}/{alias}")]
        [FrapidOutputCache(CacheProfile = "BlogContent")]
        public ActionResult Post(string categoryAlias, string alias)
        {
            var model = ContentModel.GetContent(this.Tenant, categoryAlias, alias, true);

            if (model == null)
            {
                return this.View(GetLayoutPath() + "404.cshtml");
            }

            string path = GetLayoutPath();
            string theme = this.GetTheme();
            string layout = ThemeConfiguration.GetBlogLayout(theme);

            model.LayoutPath = path;
            model.Layout = layout;

            return this.View(this.GetRazorView<AreaRegistration>("Blog/Post.cshtml"), model);
        }

        [FrapidOutputCache(CacheProfile = "BlogHome")]
        [Route("blog")]
        public ActionResult Home()
        {
            try
            {
                var contents = ContentModel.GetBlogContents();

                if (contents == null || !contents.Any())
                {
                    return this.View(GetLayoutPath() + "404.cshtml");
                }

                foreach (var content in contents)
                {
                    content.Contents = ContentExtensions.ParseHtml(this.Tenant, content.Contents);
                }

                string theme = this.GetTheme();
                string layout = ThemeConfiguration.GetBlogLayout(theme);
                var configuration = Configurations.GetDefaultConfiguration();

                var model = new Blog
                {
                    Contents = contents,
                    LayoutPath = GetLayoutPath(),
                    Layout = layout
                };

                if (configuration != null)
                {
                    model.Title = configuration.BlogTitle;
                    model.Description = configuration.BlogDescription;
                }

                return this.View(this.GetRazorView<AreaRegistration>("Blog/Home.cshtml"), model);
            }
            catch (NpgsqlException ex)
            {
                Log.Error($"An exception was encountered while trying to get blog contents. Exception: {ex}");
            }

            return null;
        }
    }
}