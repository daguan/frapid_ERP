using System;
using System.Web;
using Frapid.Framework.Extensions;
using Frapid.i18n;

namespace Frapid.Reports.Helpers
{
    public static class FormattingHelper
    {
        public static string GetFormattedValue(object value)
        {
            string cellValue = value.ToString();

            if (value is DateTime)
            {
                var date = value.To<DateTime>();
                if (date.Date == date)
                {
                    cellValue = date.ToString("d");
                }
            }

            if (value is decimal || value is double || value is float)
            {
                decimal amount = value.To<decimal>();

                cellValue =
                    amount.ToString("C", CultureManager.GetCurrentUiCulture())
                        .Replace(CultureManager.GetCurrentUiCulture().NumberFormat.CurrencySymbol, "");
            }

            return HttpUtility.HtmlEncode(cellValue);
        }
    }
}