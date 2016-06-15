using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.i18n;
using Frapid.i18n.DAL;

namespace Frapid.Web.Controllers
{
    public class ResourceController: FrapidController
    {
        [Route("i18n/resources.js")]
        [FrapidOutputCache(Duration = 31536000, VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            string culture = CultureManager.GetCurrent().TwoLetterISOLanguageName;
            string script = GetScript(this.Tenant, culture);
            return this.Content(script, "text/javascript", Encoding.UTF8);
        }

        private static string GetScript(string tenant, string culture)
        {
            var script = new StringBuilder();
            script.Append("var Resources = {");

            var resources = DbResources.GetLocalizationTable(tenant, culture);

            var resourceClassGroup = resources.GroupBy(r => r.ResourceClass).Select(group => group.ToList()).ToList();

            foreach(var resourceClass in resourceClassGroup)
            {
                int i = 0;

                foreach(var resource in resourceClass)
                {
                    if(i == 0)
                    {
                        script.Append(resource.ResourceClass + ": {");
                    }

                    script.Append(resource.Key + ": function(){ return \"");
                    string localized = resource.Translated;

                    if(string.IsNullOrWhiteSpace(localized))
                    {
                        localized = resource.Original;
                    }

                    script.Append(HttpUtility.JavaScriptStringEncode(localized));

                    script.Append("\";");
                    script.Append("},");
                    i++;
                }

                script.Append("},");
            }


            script.Append("};");

            return script.ToString();
        }
    }
}