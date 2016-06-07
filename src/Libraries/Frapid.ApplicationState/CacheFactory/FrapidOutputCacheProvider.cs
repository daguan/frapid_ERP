using System;
using System.Web.Caching;

namespace Frapid.ApplicationState.CacheFactory
{
    public sealed class FrapidOutputCacheProvider: OutputCacheProvider
    {
        public FrapidOutputCacheProvider()
        {
            this.Factory = new DefaultCacheFactory();
        }

        private ICacheFactory Factory { get; }

        public override object Get(string key)
        {
            var item = this.Factory.Get<object>(key);

            return item;
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            this.Factory.Add(key, entry, utcExpiry);
            return entry;
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            this.Factory.Add(key, entry, utcExpiry);
        }

        public override void Remove(string key)
        {
            this.Factory.Remove(key);
        }
    }
}