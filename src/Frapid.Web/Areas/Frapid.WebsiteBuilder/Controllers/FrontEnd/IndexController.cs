using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.Configuration;
using Frapid.WebsiteBuilder.Models;
using Frapid.WebsiteBuilder.Plugins;
using Frapid.WebsiteBuilder.ViewModels;
using Npgsql;
using Serilog;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    [AntiForgery]
    public class IndexController : WebsiteBuilderController
    {
        [Route("")]
        [Route("site/{categoryAlias}/{alias}")]
        [FrapidOutputCache(ProfileName = "Content")]
        public async Task<ActionResult> Index(string categoryAlias = "", string alias = "", bool isPost = false,
            FormCollection form = null)
        {
            try
            {
                Log.Verbose($"Prepping \"{this.CurrentPageUrl}\".");

                var model = this.GetContents(categoryAlias, alias, isPost, form);

                if (model == null)
                {
                    Log.Error($"Could not serve the url \"{this.CurrentPageUrl}\" because the model was null.");
                    return this.View(GetLayoutPath() + "404.cshtml");
                }

                string database = AppUsers.GetTenant();

                HostingEnvironment.QueueBackgroundWorkItem(x =>
                {
                    this.AddHit(database, model.ContentId);
                });


                Log.Verbose($"Parsing custom content extensions for \"{this.CurrentPageUrl}\".");
                model.Contents = ContentExtensions.ParseHtml(this.Tenant, model.Contents);
                Log.Verbose($"Parsing custom form extensions for \"{this.CurrentPageUrl}\".");
                model.Contents = await FormsExtension.ParseHtml(model.Contents, isPost, form);

                return this.View(this.GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
            }
            catch (NpgsqlException ex)
            {
                Log.Error("An exception was encountered while trying to get content. More info:\nCategory alias: {categoryAlias}, alias: {alias}, is post: {isPost}, form: {form}. Exception\n{ex}.",
                    categoryAlias, alias, isPost, form, ex);
                return new HttpNotFoundResult();
            }
        }

        private void AddHit(string database, int contentId)
        {
            ContentModel.AddHit(database, contentId);
        }

        private Content GetContents(string categoryAlias, string alias, bool isPost = false, FormCollection form = null)
        {
            string tenant = DbConvention.GetTenant(this.CurrentDomain);
            var model = ContentModel.GetContent(tenant, categoryAlias, alias);

            if (model == null)
            {
                return null;
            }

            bool isHomepage = string.IsNullOrWhiteSpace(categoryAlias) && string.IsNullOrWhiteSpace(alias);

            string path = GetLayoutPath();
            string layout = isHomepage ? this.GetHomepageLayout() : this.GetLayout();


            model.LayoutPath = path;
            model.Layout = layout;
            return model;
        }

        [Route("site/{categoryAlias}/{alias}")]
        [HttpPost]
        public Task<ActionResult> PostAsync(string categoryAlias, string alias, FormCollection form)
        {
            Log.Verbose($"Got a post request on \"{this.CurrentPageUrl}\". Post Data:\n\n {form}");
            return this.Index(categoryAlias, alias, true, form);
        }

        [Route("")]
        [HttpPost]
        public Task<ActionResult> PostAsync(FormCollection form)
        {
            return this.PostAsync(string.Empty, string.Empty, form);
        }
    }
}