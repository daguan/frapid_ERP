using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Frapid.i18n.Database;
using Frapid.i18n.Helpers;

namespace Frapid.i18n
{
    public class ResourceManager
    {
        private static readonly bool SuppressException = ConfigurationManager.AppSettings["SuppressMissingResourceException"].ToUpperInvariant().Equals("TRUE");

        /// <summary>
        /// Gets the localized resource.
        /// </summary>
        /// <param name="resourceClass">The name of the resource class.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture of the resource. 
        /// If this optional parameter is left empty, the current culture will be used.</param>
        /// <returns></returns>
        /// <exception cref="MissingManifestResourceException">Thrown when the resource key is not found on the specified class.</exception>
        public static string GetString(string resourceClass, string resourceKey, string cultureCode = null)
        {
            if (string.IsNullOrWhiteSpace(resourceClass))
            {
                return resourceKey;
            }

            string result = TryGetResourceFromCache(resourceClass, resourceKey, cultureCode);

            if (result == null)
            {
                if (SuppressException)
                {
                    return resourceKey;
                }

                throw new MissingManifestResourceException("Resource " + resourceClass + "." + resourceKey +
                                                           " was not found.");
            }

            return result;
        }

        /// <summary>
        /// Get the localized resource without throwing an exception if the resource is not found.
        /// </summary>
        /// <param name="resourceClass">The name of the resource class.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture of the resource. 
        /// If this optional parameter is left empty, the current culture will be used.</param>
        /// <returns></returns>
        public static string TryGetResourceFromCache(string resourceClass, string resourceKey, string cultureCode = null)
        {
            CultureInfo culture = CultureManager.GetCurrent();

            if (!string.IsNullOrWhiteSpace(cultureCode))
            {
                culture = new CultureInfo(cultureCode);
            }


            IDictionary<string, string> cache;
            object cacheItem = MemoryCache.Default.Get("Resources");

            if (cacheItem is CacheItem)
            {
                CacheItem item = (CacheItem) cacheItem;

                cache = (IDictionary<string, string>) item.Value;
            }
            else
            {
                cache = (IDictionary<string, string>) cacheItem;
            }


            if (cache == null || cache.Count.Equals(0))
            {
                InitializeResourcesAsync();
                return TryGetResourceFromCache(resourceClass, resourceKey, cultureCode);
            }

            string cacheKey = resourceClass + "." + culture.Name + "." + resourceKey;
            string result;
            cache.TryGetValue(cacheKey, out result);

            if (result != null)
            {
                return result;
            }

            //Fall back to parent culture
            while (true)
            {
                if (!string.IsNullOrWhiteSpace(culture.Parent.Name))
                {
                    cacheKey = resourceClass + "." + culture.Parent.Name + "." + resourceKey;
                    cache.TryGetValue(cacheKey, out result);

                    if (result != null)
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

        private static void InitializeResourcesAsync()
        {
            IDictionary<string, string> resources = DbResources.GetLocalizedResources();
            CacheFactory.AddToDefaultCache("Resources", resources);
        }
    }
}