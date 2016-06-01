using System.Web.Mvc;

namespace Frapid.Areas.CSRF
{
    public sealed class AntiforgeryTokenizer : IAntiforgeryTokenizer
    {
        public HtmlHelper Helper { get; set; }

        public AntiforgeryTokenizer(HtmlHelper helper)
        {
            this.Helper = helper;
        }

        public MvcHtmlString Get()
        {
            return this.Helper.AntiForgeryToken();
        }
    }
}