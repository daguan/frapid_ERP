using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class CurrencyController : DashboardController
    {
        [Route("dashboard/config/currencies")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Currency/Index.cshtml"));
        }
    }
}