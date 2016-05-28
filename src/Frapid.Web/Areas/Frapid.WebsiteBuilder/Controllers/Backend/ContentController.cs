using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class ContentController : DashboardController
    {
        [Route("dashboard/website/contents")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Index.cshtml"));
        }

        [Route("dashboard/website/contents/manage")]
        [Route("dashboard/website/contents/new")]
        [RestrictAnonymous]
        [MenuPolicy(OverridePath = "/dashboard/website/contents")]
        public async Task<ActionResult> ManageAsync(int contentId = 0)
        {
            string tenant = AppUsers.GetTenant();
            var model = await Contents.GetAsync(tenant, contentId) ?? new Content();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Manage.cshtml"), model);
        }
    }
}