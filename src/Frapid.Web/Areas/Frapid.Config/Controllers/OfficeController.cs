using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class OfficeController : DashboardController
    {
        [Route("dashboard/config/offices")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Office/Index.cshtml"));
        }
    }
}