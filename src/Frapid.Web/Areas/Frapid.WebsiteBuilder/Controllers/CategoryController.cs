using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class CategoryController : DashboardController
    {
        [Route("dashboard/website/categories")]
        [Authorize]
        public ActionResult Index(string alias = "")
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Category/Index.cshtml"));
        }
    }
}