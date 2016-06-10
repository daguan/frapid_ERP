using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class OfficeController : DashboardController
    {
        [Route("dashboard/config/offices")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Office/Index.cshtml", this.Tenant));
        }
    }
}