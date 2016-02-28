using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Areas;
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
        public async Task<ActionResult> Index(string categoryAlias = "", string alias = "", bool isPost = false, FormCollection form = null)
        {
            try
            {
                var model = this.GetContents(categoryAlias, alias, isPost, form);

                if (model == null)
                {
                    return this.View(GetLayoutPath() + "404.cshtml");
                }

                model.Contents = ContentExtensions.ParseHtml(model.Contents);
                model.Contents = await FormsExtension.ParseHtml(model.Contents, isPost, form);

                return this.View(this.GetRazorView<AreaRegistration>("Index/Index.cshtml"), model);
            }
            catch (NpgsqlException ex)
            {
                Log.Error("An exception was encountered while trying to get content. More info:\nCategory alias: {categoryAlias}, alias: {alias}, is post: {isPost}, form: {form}. Exception\n{ex}.", categoryAlias, alias, isPost, form, ex);
                return RedirectToInstallationPage();
            }
        }

        private Content GetContents(string categoryAlias, string alias, bool isPost = false, FormCollection form = null)
        {
            var model = ContentModel.GetContent(categoryAlias, alias);

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
            return this.Index(categoryAlias, alias, true, form);
        }

        [Route("")]
        [HttpPost]
        public Task<ActionResult> PostAsync(FormCollection form)
        {
            return this.PostAsync(string.Empty, string.Empty, form);

        }

        private ActionResult RedirectToInstallationPage()
        {
            string domain = DbConvention.GetDomain();

            var approved = new DomainSerializer("DomainsApproved.json");
            var installed = new DomainSerializer("DomainsInstalled.json");

            bool isApproved = approved.Get().Any(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));
            bool isInstalled = installed.Get().Any(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));

            if (isApproved && !isInstalled)
            {
                return Redirect("/install");
            }

            return Content("Frapid cannot be installed due configuration errors. Please check application log for more information.");
        }
    }
}