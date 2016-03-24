using System.Text;
using System.Web.Mvc;
using Frapid.WebsiteBuilder.Syndication.Rss;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class RssController : WebsiteBuilderController
    {
        [Route("rss/blog")]
        [Route("rss/blog/{pageNumber:int}")]
        [Route("rss/blog/{categoryAlias}")]
        [Route("rss/blog/{categoryAlias}/{pageNumber:int}")]
        public ActionResult Index(string categoryAlias = "", int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            var channel = RssModel.GetRssChannel(this.HttpContext, categoryAlias, pageNumber);
            string rss = RssWriter.Write(channel);

            return this.Content(rss, "text/xml", Encoding.UTF8);
        }
    }
}