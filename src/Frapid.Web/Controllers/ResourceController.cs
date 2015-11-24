using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Frapid.i18n;
using Frapid.i18n.Database;
using Frapid.i18n.Models;

namespace Frapid.Web.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Resource
        [Route("backend/resources.js")]
        public async Task<ActionResult> IndexAsync()
        {
            string culture = CultureManager.GetCurrent().TwoLetterISOLanguageName;
            string script = await  GetScriptAsync(culture);
            return Content(script, "text/javascript", Encoding.UTF8);
        }

        private static async Task<string> GetScriptAsync(string culture)
        {
            StringBuilder script = new StringBuilder();
            script.Append("var Resources = {");

            IEnumerable<LocalizedResource> resources = await DbResources.GetLocalizationTableAsync(culture);

            List<List<LocalizedResource>> resourceClassGroup = resources
                .GroupBy(r => r.ResourceClass)
                .Select(group => group.ToList())
                .ToList();

            foreach (List<LocalizedResource> resourceClass in resourceClassGroup)
            {
                int i = 0;

                foreach (LocalizedResource resource in resourceClass)
                {
                    if (i == 0)
                    {
                        script.Append(resource.ResourceClass + ": {");
                    }

                    script.Append(resource.Key + ": function(){ return \"");
                    string localized = resource.Translated;

                    if (string.IsNullOrWhiteSpace(localized))
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