using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class DashboardSubscriptionController : DashboardController
    {
        [Route("dashboard/website/subscriptions")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("DashboardSubscription/Index.cshtml"));
        }
    }
}