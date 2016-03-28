using System.Net;
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
        [FrapidOutputCache(ProfileName = "Sitemap.xml")]
        public ActionResult Index()
        {
            string domain = DbConvention.GetBaseDomain(this.HttpContext, true);

            if (string.IsNullOrWhiteSpace(domain))
            {
                return this.Failed("Could not generate sitemap.", HttpStatusCode.InternalServerError);
            }
            string siteMap = SiteMapGenerator.Get(domain);
            return this.Content(siteMap, "text/xml", Encoding.UTF8);
        }
    }
}