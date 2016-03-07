using System.Web.Mvc;
using Frapid.Configuration;

namespace Frapid.Areas.Caching
{
    public sealed class FrapidOutputCache : OutputCacheAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string profile = this.CacheProfile;

            if (!string.IsNullOrWhiteSpace(profile))
            {
                base.CacheProfile = string.Empty;

                string tenant = DbConvention.GetTenant();

                var config = CacheConfig.Get(tenant, profile);

                if (config != null)
                {
                    base.Duration = config.Duration;
                    base.Location = config.Location;
                    base.NoStore = config.NoStore;
                    base.SqlDependency = config.SqlDependency;
                    base.VaryByContentEncoding = config.VaryByContentEncoding;
                    base.VaryByCustom = config.VaryByCustom;
                    base.VaryByHeader = config.VaryByHeader;
                    base.VaryByParam = config.VaryByParam;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}