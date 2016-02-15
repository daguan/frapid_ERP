using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.Controllers;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class DashboardSubscriptionController : DashboardController
    {
        [Route("dashboard/website/subscriptions")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("DashboardSubscription/Index.cshtml"));
        }
    }
}