using System;

namespace Frapid.ApplicationState
{
    public interface ICacheFactory
    {
        bool Add<T>(string key, T value, DateTimeOffset expiresAt);
        T Get<T>(string key) where T : class;
    }
}