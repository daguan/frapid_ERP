using System.Web.Mvc;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.DashboardControllers
{
    public class ConfigurationProfileController : DashboardController
    {
        [Route("dashboard/account/configuration-profile")]
        [Authorize]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("ConfigurationProfile/Index.cshtml"));
        }
    }
}