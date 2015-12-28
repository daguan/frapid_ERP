using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Frapid.Configuration;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Entities;
using Npgsql;
using Content = Frapid.WebsiteBuilder.ViewModels.Content;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class IndexController : WebsiteBuilderController
    {
        [Route("")]
        [Route("site/{categoryAlias}/{alias}")]
        public ActionResult Index(string categoryAlias = "", string alias = "")
        {
            try
            {
                var content = Contents.GetPublished(categoryAlias, alias);
                Mapper.CreateMap<PublishedContentView, Content>();
                var model = Mapper.Map<Content>(content);

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
            catch (NpgsqlException)
            {
                return RedirectToInstallationPage();
            }
        }

        private ActionResult RedirectToInstallationPage()
        {
            string domain = DbConvention.GetDomain();

            var approved = new DomainSerializer("DomainsApproved.json");
            var installed = new DomainSerializer("DomainsInstalled.json");

            bool isApproved = approved.Get().Any(x => x.DomainName.Equals(domain));
            bool isInstalled = installed.Get().Any(x => x.DomainName.Equals(domain));

            if (isApproved && !isInstalled)
            {
                return Redirect("/install");
            }

            return Content("Frapid cannot be installed due configuration errors. Please check application log for more information.");
        }
    }
}