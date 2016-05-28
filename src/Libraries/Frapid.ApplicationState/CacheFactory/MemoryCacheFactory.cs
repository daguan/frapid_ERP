using System;
using System.Runtime.Caching;

namespace Frapid.ApplicationState.CacheFactory
{
    public class MemoryCacheFactory: ICacheFactory
    {
        public bool Add<T>(string key, T value, DateTimeOffset expiresAt)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if(value == null)
            {
                return false;
            }

            var cacheItem = new CacheItem(key, value);

            if(MemoryCache.Default[key] == null)
            {
                MemoryCache.Default.Add(cacheItem, new CacheItemPolicy());
            }
            else
            {
                MemoryCache.Default[key] = cacheItem;
            }

            return true;
        }

        public T Get<T>(string key) where T : class
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            return MemoryCache.Default.Get(key) as T;
        }
    }
}