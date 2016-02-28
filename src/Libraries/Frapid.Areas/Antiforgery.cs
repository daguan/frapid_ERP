using System.Web.Mvc;
using Frapid.Configuration;

namespace Frapid.Areas
{
    public static class Antiforgery
    {
        public static MvcHtmlString GetAntiForgeryToken(this HtmlHelper helper)
        {
            if (DbConvention.IsStaticDomain())
            {
                return new MvcHtmlString(string.Empty);
            }

            return helper.AntiForgeryToken();
        }
    }
}