using System;
using System.Text;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Caching;
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
            string siteMap = SiteMapGenerator.Get(domain);
            return this.Content(siteMap, "text/xml", Encoding.UTF8);
        }
    }
}