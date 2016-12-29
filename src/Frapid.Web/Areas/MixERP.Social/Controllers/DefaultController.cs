using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;
using MixERP.Social.DTO;
using MixERP.Social.Models;

namespace MixERP.Social.Controllers
{
    public sealed class DefaultController : DashboardController
    {
        [Route("dashboard/social")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/social")]
        [RestrictAnonymous]
        [MenuPolicy]
        [HttpPost]
        public async Task<ActionResult> PostAsync(Feed model)
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);
            var feedResult = await Feeds.PostAsync(this.Tenant, model, meta).ConfigureAwait(true);
            return this.Ok(feedResult);
        }

        [Route("dashboard/social/delete/{feedId}")]
        [RestrictAnonymous]
        [MenuPolicy(OverridePath = "/dashboard/social")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(long feedId)
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);

            try
            {
                await Feeds.DeleteAsync(this.Tenant, feedId, meta).ConfigureAwait(true);
                return this.Ok();
            }
            catch (FeedException ex)
            {
                return this.Failed(ex.Message, HttpStatusCode.Forbidden);
            }
        }

        [Route("dashboard/social/feeds")]
        [Route("dashboard/social/feeds/{lastFeedId}")]
        [Route("dashboard/social/feeds/{lastFeedId}/{parentFeedId}")]
        [RestrictAnonymous]
        [MenuPolicy(OverridePath = "/dashboard/social")]
        public async Task<ActionResult> GetNextTopFeedsAsync(long lastFeedId = 0, long parentFeedId = 0)
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);
            var model = await DAL.Feeds.GetFeedsAsync(this.Tenant, meta.UserId, lastFeedId, parentFeedId).ConfigureAwait(true);

            return this.Ok(model);
        }
    }
}