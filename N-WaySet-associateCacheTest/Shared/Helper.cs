using N_WaySet_associateCache.Cache;
using N_WaySet_associateCache.EvictionPolicy;

namespace N_WaySet_associateCacheTest.Shared
{
    public static class CacheHelper
    {
        public static ICache<int, int> CreateIntLRU(int ways = 3, int sets = 3) => CreateLRU<int, int>(ways, sets);
        public static ICache<TKey, TValue> CreateLRU<TKey, TValue>(int ways = 3, int sets = 3)
        {
            return new NWaySetCache<TKey, TValue>(new LRU<TKey>(), ways, sets);
        }
        
        public static ICache<int, int> CreateIntMRU(int ways = 3, int sets = 3) => CreateMRU<int, int>(ways, sets);
        public static ICache<TKey, TValue> CreateMRU<TKey, TValue>(int ways = 3, int sets = 3)
        {
            return new NWaySetCache<TKey, TValue>(new MRU<TKey>(), ways, sets);
        }
    }
}