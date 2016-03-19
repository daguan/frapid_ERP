using System;
using Frapid.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Frapid.Framework
{
    public class RedisCacheFactory : ICacheFactory
    {
        public static IDatabase GetDb()
        {
            string cs = RedisConnectionString.GetConnectionString();
            var redis = ConnectionMultiplexer.Connect(cs);
            return redis.GetDatabase();
        }

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
            if (serializedObject.IsNull)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}