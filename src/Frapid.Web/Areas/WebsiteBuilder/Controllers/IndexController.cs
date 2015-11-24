using System.Web.Mvc;
using WebsiteBuilder.Models;

namespace WebsiteBuilder.Controllers
{
    public class IndexController : Controller
    {
        [Route("")]
        [Route("site/{*alias}")]
        public ActionResult Index(string alias = "")
        {
            Content model = DAL.Content.Get(alias);
            return View("~/Areas/WebsiteBuilder/Views/Index/Index.cshtml", model);
        }
    }
}