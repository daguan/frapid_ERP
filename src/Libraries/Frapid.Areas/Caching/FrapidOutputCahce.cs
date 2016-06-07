using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Frapid.Configuration;
using Serilog;

namespace Frapid.Areas.Caching
{
    public sealed class FrapidOutputCache : DonutOutputCacheAttribute
    {
        /// <summary>
        /// Gets or sets the cache profile name.
        /// </summary>
        public string ProfileName { get; set; }

        private string GetCacheKey(ControllerContext context)
        {
            string tenant = string.Empty;
            var request = context.RequestContext.HttpContext.Request;
            if (request?.Url != null)
            {
                Log.Verbose("Getting cache key for the current request: {Url}.", request.Url);
                string url = request.Url.Authority;
                tenant = TenantConvention.GetTenant(url);
            }

            string key = this.KeyGenerator.GenerateKey(context, this.CacheSettings);

            if (!string.IsNullOrWhiteSpace(tenant))
            {
                key = tenant + "." + key;
            }

            Log.Verbose("The cache key for the current request is {key}.", key);
            return key;
        }

        private CacheSettings GetCacheSettings()
        {
            Log.Verbose("Getting cache settings by tenant based profile.");

            var settings = new CacheSettings
            {
                Duration = this.Duration,
                Location = this.Location,
                NoStore = this.NoStore,
                Options = this.Options,
                VaryByCustom = this.VaryByCustom,
                VaryByParam = this.VaryByParam
            };

            string profile = this.ProfileName;

            if (string.IsNullOrWhiteSpace(profile))
            {
                Log.Verbose("Aborted creating cache settings because the current cache profile name is empty.");
                return settings;
            }

            Log.Verbose("Setting the \"CacheProfile\" to an empty value to override the behavior of DonutCache.");
            this.CacheProfile = string.Empty;

            string tenant = TenantConvention.GetTenant();
            Log.Verbose("The current tenant is {tenant}.", tenant);

            Log.Verbose("Getting cache config for current tenant \"{tenant}\" and profile \"{profile}\".", tenant, profile);
            var config = CacheConfig.Get(tenant, profile);

            if (config == null)
            {
                Log.Verbose("Could not find any cache config information for current tenant \"{tenant}\" and profile \"{profile}\".", tenant, profile);
                return settings;
            }

            settings.IsCachingEnabled = true;

            Log.Verbose("The cache duration for tenant \"{tenant}\" and profile \"{profile}\" is {Duration}", tenant, profile, config.Duration);
            settings.Duration = config.Duration;

            Log.Verbose("The cache location for tenant \"{tenant}\" and profile \"{profile}\" is {Location}", tenant, profile, config.Location);
            settings.Location = config.Location;

            Log.Verbose("The cache NoStore value for tenant \"{tenant}\" and profile \"{profile}\" is {NoStore}", tenant, profile, config.NoStore);
            settings.NoStore = config.NoStore;

            Log.Verbose("The cache VaryByCustom value for tenant \"{tenant}\" and profile \"{profile}\" is {VaryByCustom}", tenant, profile, config.VaryByCustom);
            settings.VaryByCustom = config.VaryByCustom;

            Log.Verbose("The cache VaryByParam value for tenant \"{tenant}\" and profile \"{profile}\" is {VaryByParam}", tenant, profile, config.VaryByParam);
            settings.VaryByParam = config.VaryByParam;

            Log.Verbose("The cache Options value for tenant \"{tenant}\" and profile \"{profile}\" is {Options}", tenant, profile, config.Options);
            settings.Options = config.Options;

            return settings;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (this.CacheSettings == null)
            {
                Log.Verbose("Current cache settings is null.");
                return;
            }

            Log.Verbose("Executing callback action.");
            this.ExecuteCallback(filterContext, filterContext.Exception != null);

            // If we are in the context of a child action, the main action is responsible for setting
            // the right HTTP Cache headers for the final response.
            Log.Verbose("Determining if current action is a child action.");
            if (!filterContext.IsChildAction)
            {
                Log.Verbose("The current action is not a child action. Setting cache header values.");
                this.CacheHeadersHelper.SetCacheHeaders(filterContext.HttpContext.Response, this.CacheSettings);
            }
        }

        /// <summary>
        ///     Executes the callback.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="hasErrors">if set to <c>true</c> [has errors].</param>
        private void ExecuteCallback(ControllerContext context, bool hasErrors)
        {
            string cacheKey = this.GetCacheKey(context);

            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }

            Log.Verbose("Getting callback info for cache key \"{cacheKey}\".", cacheKey);
            var callback = context.HttpContext.Items[cacheKey] as Action<bool>;

            Log.Verbose("Invoking callback action for cache key \"{cacheKey}\".", cacheKey);
            callback?.Invoke(hasErrors);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.CacheSettings = this.GetCacheSettings();
            string cacheKey = this.GetCacheKey(filterContext);

            // If we are unable to generate a cache key it means we can't do anything
            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }

            // Are we actually storing data on the server side ?
            if (this.CacheSettings.IsServerCachingEnabled)
            {
                Log.Verbose("Server caching is enabled for key: \"{cacheKey}\".", cacheKey);

                CacheItem cachedItem = null;

                // If the request is a POST, we lookup for NoCacheLookupForPosts option
                // We are fetching the stored value only if the option has not been set and the request is not a POST
                if ((this.CacheSettings.Options & OutputCacheOptions.NoCacheLookupForPosts) !=
                    OutputCacheOptions.NoCacheLookupForPosts ||
                    filterContext.HttpContext.Request.HttpMethod != "POST")
                {
                    Log.Verbose("Fetching cache value from output cache manager.");
                    cachedItem = this.OutputCacheManager.GetItem(cacheKey);
                }

                // We have a cached version on the server side
                if (cachedItem != null)
                {
                    // We inject the previous result into the MVC pipeline
                    // The MVC action won't execute as we injected the previous cached result.
                    filterContext.Result = new ContentResult
                    {
                        Content =
                            this.DonutHoleFiller.ReplaceDonutHoleContent(cachedItem.Content, filterContext,
                                this.CacheSettings.Options),
                        ContentType = cachedItem.ContentType
                    };
                }
            }

            // Did we already injected something ?
            if (filterContext.Result != null)
            {
                return; // No need to continue 
            }

            // We are hooking into the pipeline to replace the response Output writer
            // by something we own and later eventually gonna cache
            var cachingWriter = new StringWriter(CultureInfo.InvariantCulture);

            var originalWriter = filterContext.HttpContext.Response.Output;

            filterContext.HttpContext.Response.Output = cachingWriter;

            // Will be called back by OnResultExecuted -> ExecuteCallback
            filterContext.HttpContext.Items[cacheKey] = new Action<bool>
                (
                hasErrors => { this.Callback(filterContext, hasErrors, cacheKey, originalWriter, cachingWriter); });
        }

        private void Callback(ActionExecutingContext filterContext, bool hasErrors, string cacheKey,
            TextWriter originalWriter, StringWriter cachingWriter)
        {
            // Removing this executing action from the context
            filterContext.HttpContext.Items.Remove(cacheKey);

            // We restore the original writer for response
            filterContext.HttpContext.Response.Output = originalWriter;

            if (hasErrors)
            {
                return; // Something went wrong, we are not going to cache something bad
            }

            // Now we use owned caching writer to actually store data
            var cacheItem = new CacheItem
            {
                Content = cachingWriter.ToString(),
                ContentType = filterContext.HttpContext.Response.ContentType
            };

            filterContext.HttpContext.Response.Write(this.DonutHoleFiller.RemoveDonutHoleWrappers(cacheItem.Content,
                filterContext, this.CacheSettings.Options));

            if (this.CacheSettings.IsServerCachingEnabled &&
                filterContext.HttpContext.Response.StatusCode == 200)
            {
                this.OutputCacheManager.AddItem(cacheKey, cacheItem,
                    DateTime.UtcNow.AddSeconds(this.CacheSettings.Duration));
            }
        }
    }
}