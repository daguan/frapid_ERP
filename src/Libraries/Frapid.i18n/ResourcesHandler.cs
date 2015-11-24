using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Frapid.i18n.Database;
using Frapid.i18n.Models;

namespace Frapid.i18n
{
    /// <summary>
    ///     Serializes the localized resources as a Javascript file. Use web.config to register the handler mapping.
    /// </summary>
    public class ResourcesHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable => true;

        public async void ProcessRequest(HttpContext context)
        {
            string culture = context.Request.QueryString["culture"];
            string script = await GetScriptAsync(culture);

            context.Response.ContentType = "text/javascript";

            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(30, 0, 0, 0));

            context.Response.Write(script);
            context.Response.End();
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

        #endregion
    }
}