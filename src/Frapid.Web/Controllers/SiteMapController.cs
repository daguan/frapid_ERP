using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.Configuration;
using Frapid.Framework;

namespace Frapid.Web.Controllers
{
    public class SiteMapController : FrapidController
    {
        [Route("sitemap.xml")]
        [FrapidOutputCache(Duration = 30)]
        public ActionResult Index()
        {
            string domain = this.Request.Url?.GetLeftPart(UriPartial.Authority);
            var approved = new DomainSerializer("DomainsApproved.json");
            var tenant = approved.Get().FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));
            string domainName = domain;

            if (tenant != null)
            {
                string protocol = this.Request.IsSecureConnection ? "https://" : "http://";
                domainName = protocol + tenant.DomainName;
            }

            string siteMap = SiteMapGenerator.Get(domainName);
            return this.Content(siteMap, "text/xml", Encoding.UTF8);
        }
    }
}