using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;

#if DNX451 || NET45 || NET40
using System.Runtime.Caching;
#endif

namespace Frapid.NPoco
{
    /// <summary>
    /// Container for a Memory cache object
    /// </summary>
    /// <remarks>
    /// Better to have one memory cache instance than many so it's memory management can be handled more effectively
    /// http://stackoverflow.com/questions/8463962/using-multiple-instances-of-memorycache
    /// </remarks>

    internal class ManagedCache
    {
#if !NET35 
        public MemoryCache GetCache()
        {
            return ObjectCache;
        }
    #if DNXCORE50
        static readonly MemoryCache ObjectCache = new MemoryCache(new MemoryCacheOptions());
    #else
        static readonly MemoryCache ObjectCache = new MemoryCache("NPoco");
    #endif
#endif
    }

    public class Cache<TKey, TValue>
    {
        private readonly bool _useManaged;

        private Cache(bool useManaged)
        {
            this._useManaged = useManaged;
        }

        /// <summary>
        /// Creates a cache that uses static storage
        /// </summary>
        /// <returns></returns>
        public static Cache<TKey, TValue> CreateStaticCache()
        {
            return new Cache<TKey, TValue>(false);
        }

        public static Cache<TKey, TValue> CreateManagedCache()
        {
            return new Cache<TKey, TValue>(true);
        }

        readonly Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        readonly ManagedCache _managedCache = new ManagedCache();
        
        public int Count => this._map.Count;

        public TValue Get(TKey key, Func<TValue> factory)
        {
#if !NET35 && !DNXCORE50 
            if (this._useManaged)
            {
                MemoryCache objectCache = this._managedCache.GetCache();
                //lazy usage of AddOrGetExisting ref: http://stackoverflow.com/questions/10559279/how-to-deal-with-costly-building-operations-using-memorycache/15894928#15894928
                Lazy<TValue> newValue = new Lazy<TValue>(factory);
                // the line belows returns existing item or adds the new value if it doesn't exist
                
                Lazy<TValue> value = (Lazy<TValue>)objectCache.AddOrGetExisting(key.ToString(), newValue, new System.Runtime.Caching.CacheItemPolicy
                {
                    //sliding expiration of 1 hr, if the same key isn't used in this 
                    // timeframe it will be removed from the cache
                    SlidingExpiration = new TimeSpan(1,0,0)
                });
                return (value ?? newValue).Value; // Lazy<T> handles the locking itself
            }
#endif

            // Check cache
            this._lock.EnterReadLock();
            TValue val;
            try
            {
                if (this._map.TryGetValue(key, out val))
                    return val;
            }
            finally
            {
                this._lock.ExitReadLock();
            }

            // Cache it
            this._lock.EnterWriteLock();
            try
            {
                // Check again
                if (this._map.TryGetValue(key, out val))
                    return val;

                // Create it
                val = factory();

                // Store it
                this._map.Add(key, val);

                // Done
                return val;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public bool AddIfNotExists(TKey key, TValue value)
        {
            // Cache it
            this._lock.EnterWriteLock();
            try
            {
                // Check again
                TValue val;
                if (this._map.TryGetValue(key, out val))
                    return true;

                // Create it
                val = value;

                // Store it
                this._map.Add(key, val);

                // Done
                return false;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public void Flush()
        {
            // Cache it
            this._lock.EnterWriteLock();
            try
            {
                this._map.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

        }
    }
}
