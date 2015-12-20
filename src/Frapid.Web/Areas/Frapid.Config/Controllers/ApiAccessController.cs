using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class ApiAccessController : DashboardController
    {
        [Route("dashboard/config/api-access")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("ApiAccess/Index.cshtml"));
        }
    }
}