using System.Threading.Tasks;
using System.Web.Mvc;
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
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/website/contents/manage")]
        [Route("dashboard/website/contents/new")]
        [RestrictAnonymous]
        [MenuPolicy(OverridePath = "/dashboard/website/contents")]
        public async Task<ActionResult> ManageAsync(int contentId = 0)
        {
            var model = await Contents.GetAsync(this.Tenant, contentId).ConfigureAwait(false) ?? new Content();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Manage.cshtml", this.Tenant), model);
        }
    }
}