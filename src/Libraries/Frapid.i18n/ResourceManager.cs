using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Runtime.Caching;
using Frapid.i18n.DAL;
using Frapid.i18n.Helpers;

namespace Frapid.i18n
{
    public class ResourceManager
    {
        private static readonly bool SuppressException = ConfigurationManager.AppSettings["SuppressMissingResourceException"].ToUpperInvariant().Equals("TRUE");

        /// <summary>
        ///     Gets the localized resource.
        /// </summary>
        /// <param name="tenant">The name of the database or tenant.</param>
        /// <param name="resourceClass">The name of the resource class.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">
        ///     The culture of the resource.
        ///     If this optional parameter is left empty, the current culture will be used.
        /// </param>
        /// <returns></returns>
        /// <exception cref="MissingManifestResourceException">Thrown when the resource key is not found on the specified class.</exception>
        public static string GetString(string tenant, string resourceClass, string resourceKey, string cultureCode = null)
        {
            if(string.IsNullOrWhiteSpace(resourceClass))
            {
                return resourceKey;
            }

            string result = TryGetResourceFromCache(tenant, resourceClass, resourceKey, cultureCode);

            if(result != null)
            {
                return result;
            }

            if(SuppressException)
            {
                return resourceKey;
            }

            throw new MissingManifestResourceException("Resource " + resourceClass + "." + resourceKey + " was not found.");
        }

        private static CultureInfo GetCulture(string cultureCode)
        {
            var culture = CultureManager.GetCurrent();

            if(!string.IsNullOrWhiteSpace(cultureCode))
            {
                culture = new CultureInfo(cultureCode);
            }

            return culture;
        }

        private static IDictionary<string, string> GetCache(string tenant)
        {
            IDictionary<string, string> cache;
            var cacheItem = MemoryCache.Default.Get("Resources");

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if(cacheItem is CacheItem)
            {
                var item = (CacheItem)cacheItem;
                cache = (IDictionary<string, string>)item.Value;
            }
            else
            {
                cache = (IDictionary<string, string>)cacheItem;
            }


            if(cache != null &&
               cache.Count > 0)
            {
                return cache;
            }

            InitializeResourcesAsync(tenant);
            return GetCache(tenant);
        }

        /// <summary>
        ///     Get the localized resource without throwing an exception if the resource is not found.
        /// </summary>
        /// <param name="tenant">The name of the current tenant or database.</param>
        /// <param name="resourceClass">The name of the resource class.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">
        ///     The culture of the resource.
        ///     If this optional parameter is left empty, the current culture will be used.
        /// </param>
        /// <returns></returns>
        public static string TryGetResourceFromCache(string tenant, string resourceClass, string resourceKey, string cultureCode = null)
        {
            while(true)
            {
                var culture = GetCulture(cultureCode);
                var cache = GetCache(tenant);

                string cacheKey = resourceClass + "." + culture.Name + "." + resourceKey;
                string result;
                cache.TryGetValue(cacheKey, out result);

                if(result != null)
                {
                    return result;
                }

                //Fall back to parent culture
                while(true)
                {
                    if(!string.IsNullOrWhiteSpace(culture.Parent.Name))
                    {
                        cacheKey = resourceClass + "." + culture.Parent.Name + "." + resourceKey;
                        cache.TryGetValue(cacheKey, out result);

                        if(result != null)
                        {
                            return result;
                        }

                        culture = culture.Parent;
                        continue;
                    }

                    break;
                }

                //Fall back to invariant culture
                cacheKey = resourceClass + "." + resourceKey;
                cache.TryGetValue(cacheKey, out result);

                return result;
            }
        }

        private static void InitializeResourcesAsync(string tenant)
        {
            IDictionary<string, string> resources = DbResources.GetLocalizedResources(tenant);
            CacheFactory.AddToDefaultCache("Resources", resources);
        }
    }
}