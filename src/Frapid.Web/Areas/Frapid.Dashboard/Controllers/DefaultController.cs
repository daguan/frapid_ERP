using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.Authorization;
using Frapid.i18n;

namespace Frapid.Dashboard.Controllers
{
    public class DefaultController: DashboardController
    {
        [Route("dashboard")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return View(GetRazorView<AreaRegistration>("Default/Index.cshtml"));
        }

        [Route("dashboard/meta")]
        [RestrictAnonymous]
        public async Task<ActionResult> GetMetaAsync()
        {
            var curent = await AppUsers.GetCurrentAsync();

            return this.Ok
                (
                 new ViewModels.Dashboard
                 {
                     Culture = CultureManager.GetCurrent().Name,
                     Language = CultureManager.GetCurrent().TwoLetterISOLanguageName,
                     JqueryUIi18NPath = "/Scripts/jquery-ui/i18n/",
                     Today = DateTimeOffset.UtcNow.ToString("D"),
                     Now = DateTimeOffset.UtcNow.ToString(CultureManager.GetCurrent()),
                     UserId = curent.UserId,
                     User = curent.Email,
                     Office = curent.Office,
                     MetaView = curent,
                     ShortDateFormat = CultureManager.GetShortDateFormat(),
                     LongDateFormat = CultureManager.GetLongDateFormat(),
                     ThousandSeparator = CultureManager.GetThousandSeparator(),
                     DecimalSeparator = CultureManager.GetDecimalSeparator(),
                     CurrencyDecimalPlaces = CultureManager.GetCurrencyDecimalPlaces(),
                     CurrencySymbol = CultureManager.GetCurrencySymbol(),
                     DatepickerFormat = CultureManager.GetCurrent().DateTimeFormat.ShortDatePattern,
                     DatepickerShowWeekNumber = true,
                     DatepickerWeekStartDay = (int)CultureManager.GetCurrent().DateTimeFormat.FirstDayOfWeek,
                     DatepickerNumberOfMonths = "[2, 3]"
                 });
        }
    }
}