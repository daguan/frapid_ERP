using System;
using Frapid.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Frapid.ApplicationState.CacheFactory
{
    public class RedisCacheFactory: ICacheFactory
    {
        public static ConnectionMultiplexer Redis { get; private set; }

        public bool Add<T>(string key, T value, DateTimeOffset expiresAt)
        {
            string serializedObject = JsonConvert.SerializeObject(value);
            var expiration = expiresAt.Subtract(DateTimeOffset.Now);
            var db = GetDb();
            return db.StringSet(key, serializedObject, expiration);
        }

        public T Get<T>(string key) where T : class
        {
            var db = GetDb();
            var serializedObject = db.StringGet(key);
            if(serializedObject.IsNull)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public static IDatabase GetDb()
        {
            if(Redis == null)
            {
                string cs = RedisConnectionString.GetConnectionString();
                Redis = ConnectionMultiplexer.Connect(cs);
            }

            return Redis.GetDatabase();
        }
    }
}