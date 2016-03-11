using System.Web.Mvc;
using Frapid.Configuration;

namespace Frapid.Areas.Caching
{
    public sealed class FrapidOutputCache : OutputCacheAttribute
    {
        public FrapidOutputCache()
        {
            this.Duration = 0;
        }

        public string ProfileName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string profile = this.ProfileName;

            if (!string.IsNullOrWhiteSpace(profile))
            {
                this.CacheProfile = string.Empty;

                string tenant = DbConvention.GetTenant();

                var config = CacheConfig.Get(tenant, profile);

                if (config == null || this.IsStatic(context))
                {
                    context.HttpContext.Response.Cache.SetNoStore();
                }
                else
                {
                    this.Duration = config.Duration;
                    this.Location = config.Location;
                    this.NoStore = config.NoStore;
                    this.SqlDependency = config.SqlDependency;
                    this.VaryByContentEncoding = config.VaryByContentEncoding;
                    this.VaryByCustom = config.VaryByCustom;
                    this.VaryByHeader = config.VaryByHeader;
                    this.VaryByParam = config.VaryByParam;
                }
            }

            base.OnActionExecuting(context);
        }

        private bool IsStatic(ActionExecutingContext context)
        {
            string domain = context.RequestContext.HttpContext.Request.Url?.DnsSafeHost;

            if (string.IsNullOrWhiteSpace(domain))
            {
                return false;
            }

            return DbConvention.IsStaticDomain(domain);
        }
    }
}