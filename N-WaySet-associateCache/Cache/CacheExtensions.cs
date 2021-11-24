using System;

namespace N_WaySet_associateCache.Cache
{
    public static class CacheExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(
            this ICache<TKey, TValue> cache, 
            TKey key,
            Func<TValue> createValue) =>
            cache.Get(key).Match(value => value, () =>
            {
                var value = createValue();
                cache.Add(key, value);
                return value;
            });
    }
}