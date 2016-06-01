using System.Web.Mvc;
using Frapid.Areas.CSRF;
using Frapid.Configuration;
using Frapid.Configuration.TenantServices;
using Frapid.Framework;
using Serilog;

namespace Frapid.Areas
{
    public static class AntiforgeryHelper
    {
        private static string GetDomainName()
        {
            string url = FrapidHttpContext.GetCurrent().Request.Url.Authority;
            var extractor = new DomainNameExtractor(Log.Logger);
            return extractor.GetDomain(url);
        }

        public static MvcHtmlString GetAntiForgeryToken(this HtmlHelper helper)
        {
            var logger = Log.Logger;
            var serializer = new DomainSerializer("DomainsApproved.json");
            var check = new StaticDomainCheck(logger, serializer);
            var tokenizer = new AntiforgeryTokenizer(helper);
            string currentDomain = GetDomainName();

            var generator = new AntiforgeryTokenGenerator(check, tokenizer, currentDomain);
            return generator.GetAntiForgeryToken();
        }
    }
}