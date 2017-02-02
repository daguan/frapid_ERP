using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.CSRF;
using Frapid.Calendar.Models;
using Frapid.Calendar.ViewModels;

namespace Frapid.Calendar.Controllers.Backend
{
    [AntiForgery]
    public sealed class CalendarController : CalendarBackendController
    {
        [Route("dashboard/calendar")]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Backend/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/calendar")]
        [HttpPost]
        public async Task<ActionResult> PostAsync(CalendarEvent calendarEvent)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState(this.ModelState);
            }

            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);

            calendarEvent.UserId = meta.UserId;
            calendarEvent.AuditUserId = meta.UserId;
            calendarEvent.AuditTs = DateTimeOffset.UtcNow;

            try
            {
                var result = CalendarEntry.AddEntryAsync(this.Tenant, calendarEvent);

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.Failed(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}