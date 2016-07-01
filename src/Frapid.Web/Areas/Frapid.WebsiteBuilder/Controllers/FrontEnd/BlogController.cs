using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Areas.Caching;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Models;
using Frapid.WebsiteBuilder.ViewModels;
using Npgsql;
using Serilog;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class BlogController : WebsiteBuilderController
    {
        [Route("blog/{categoryAlias}/{alias}/hit")]
        [HttpPost]
        public async Task<ActionResult> CounterAsync(string categoryAlias = "", string alias = "")
        {
            await ContentModel.AddHitAsync(this.Tenant, categoryAlias, alias).ConfigureAwait(false);
            return this.Ok();
        }

        [Route("blog/{categoryAlias}/{alias}")]
        [FrapidOutputCache(ProfileName = "BlogContent")]
        public async Task<ActionResult> PostAsync(string categoryAlias, string alias)
        {
            var model =
                await ContentModel.GetContentAsync(this.Tenant, categoryAlias, alias, true).ConfigureAwait(true);

            if (model == null)
            {
                return this.View(GetLayoutPath(this.Tenant) + "404.cshtml");
            }

            string path = GetLayoutPath(this.Tenant);
            string theme = this.GetTheme();
            string layout = ThemeConfiguration.GetBlogLayout(this.Tenant, theme);

            model.LayoutPath = path;
            model.Layout = layout;
            model.Contents = await ContentExtensions.ParseHtmlAsync(this.Tenant, model.Contents).ConfigureAwait(true);

            return this.View(this.GetRazorView<AreaRegistration>("Blog/Post.cshtml", this.Tenant), model);
        }

        [FrapidOutputCache(ProfileName = "BlogHome")]
        [Route("blog")]
        [Route("blog/{pageNumber}")]
        public async Task<ActionResult> HomeAsync(int pageNumber = 1)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var contents = (await ContentModel.GetBlogContentsAsync(this.Tenant, pageNumber).ConfigureAwait(true)).ToList();

                if (!contents.Any())
                {
                    return this.View(GetLayoutPath(this.Tenant) + "404.cshtml");
                }

                foreach (var content in contents)
                {
                    content.Contents =
                        await ContentExtensions.ParseHtmlAsync(this.Tenant, content.Contents).ConfigureAwait(false);
                }

                string theme = this.GetTheme();
                string layout = ThemeConfiguration.GetBlogLayout(this.Tenant, theme);

                var configuration = await Configurations.GetDefaultConfigurationAsync(this.Tenant).ConfigureAwait(false);

                var model = new Blog
                {
                    Contents = contents,
                    LayoutPath = GetLayoutPath(this.Tenant),
                    Layout = layout
                };

                if (configuration != null)
                {
                    model.Title = configuration.BlogTitle;
                    model.Description = configuration.BlogDescription;
                }

                return this.View(this.GetRazorView<AreaRegistration>("Blog/Home.cshtml", this.Tenant), model);
            }
            catch (NpgsqlException ex)
            {
                Log.Error($"An exception was encountered while trying to get blog contents. Exception: {ex}");
            }

            return null;
        }
    }
}