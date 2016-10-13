using System;
using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.i18n;

namespace Frapid.Dashboard.Controllers
{
    public class DefaultController : DashboardController
    {
        [Route("dashboard")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return View(GetRazorView<AreaRegistration>("Default/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/meta")]
        [RestrictAnonymous]
        public ActionResult GetMeta()
        {
            return this.Ok
                (
                    new ViewModels.Dashboard
                    {
                        Culture = CultureManager.GetCurrent().Name,
                        Language = CultureManager.GetCurrent().TwoLetterISOLanguageName,
                        JqueryUIi18NPath = "/Scripts/jquery-ui/i18n/",
                        Today = DateTime.Today.Date.ToString("O"),
                        Now = DateTimeOffset.UtcNow.ToString("O"),
                        UserId = this.AppUser.UserId,
                        User = this.AppUser.Email,
                        Office = this.AppUser.OfficeName,
                        MetaView = this.AppUser,
                        ShortDateFormat = CultureManager.GetShortDateFormat(),
                        LongDateFormat = CultureManager.GetLongDateFormat(),
                        ThousandSeparator = CultureManager.GetThousandSeparator(),
                        DecimalSeparator = CultureManager.GetDecimalSeparator(),
                        CurrencyDecimalPlaces = CultureManager.GetCurrencyDecimalPlaces(),
                        CurrencySymbol = CultureManager.GetCurrencySymbol(),
                        DatepickerFormat = CultureManager.GetCurrent().DateTimeFormat.ShortDatePattern,
                        DatepickerShowWeekNumber = true,
                        DatepickerWeekStartDay = (int) CultureManager.GetCurrent().DateTimeFormat.FirstDayOfWeek,
                        DatepickerNumberOfMonths = "[2, 3]"
                    });
        }
    }
}