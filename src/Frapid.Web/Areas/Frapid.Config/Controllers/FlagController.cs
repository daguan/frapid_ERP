using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class FlagController : DashboardController
    {
        [Route("dashboard/config/flags")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Flag/Index.cshtml"));
        }
    }
}