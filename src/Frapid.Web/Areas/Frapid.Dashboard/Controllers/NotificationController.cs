using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.CSRF;
using Frapid.Dashboard.DAL;

namespace Frapid.Dashboard.Controllers
{
    [AntiForgery]
    public sealed class NotificationController : BackendController
    {
        [Route("dashboard/my/notifications")]
        public async Task<ActionResult> GetMyNotificationAsync()
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);
            var model = await Notifications.GetMyNotificationsAsync(this.Tenant, meta.LoginId).ConfigureAwait(true);

            return this.Ok(model);
        }

        [Route("dashboard/my/notifications/set-seen/{notificationId}")]
        [HttpPost]
        public async Task<ActionResult> GetMyNotificationAsync(Guid notificationId)
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);
            await Notifications.MarkAsSeenAsync(this.Tenant, notificationId, meta.UserId).ConfigureAwait(true);

            return this.Ok();
        }
    }
}