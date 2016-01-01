using System;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.i18n;

namespace Frapid.Dashboard.Controllers
{
    public class DefaultController : DashboardController
    {
        [Route("dashboard")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return View(GetRazorView<AreaRegistration>("Default/Index.cshtml"));
        }

        [Route("dashboard/meta")]
        [RestrictAnonymous]
        public ActionResult GetMeta()
        {
            var curent = AppUsers.GetCurrent();

            return Json(new ViewModels.Dashboard
            {
                Culture = CultureManager.GetCurrent().Name,
                Language = CultureManager.GetCurrent().TwoLetterISOLanguageName,
                JqueryUIi18NPath = "/Scripts/jquery-ui/i18n/",
                Today = DateTime.Now.ToShortDateString(),
                Now = DateTime.Now.ToString(CultureManager.GetCurrent()),
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
                DatepickerWeekStartDay = (int) CultureManager.GetCurrent().DateTimeFormat.FirstDayOfWeek,
                DatepickerNumberOfMonths = "[2, 3]"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}