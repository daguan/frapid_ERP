using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class ContactController : DashboardController
    {
        [Route("dashboard/website/contacts")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Contact/Index.cshtml"));
        }
    }
}