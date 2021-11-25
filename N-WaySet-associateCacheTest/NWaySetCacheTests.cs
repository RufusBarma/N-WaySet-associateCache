using System.Linq;
using LanguageExt.UnsafeValueAccess;
using N_WaySet_associateCache.Cache;
using N_WaySet_associateCacheTest.Shared;
using Xunit;

namespace N_WaySet_associateCacheTest
{
    // ReSharper disable once InconsistentNaming
    public class NWaySetCacheTests
    {
        [Fact]
        public void LRU_AddGet_Test()
        {
            var cache = CacheHelper.CreateIntLRU();
            cache.Add(1, 1);
            var result = cache.Get(1).Value();
            Assert.Equal(1, result);
        }

        [Fact]
        public void MRU_AddGet_Test()
        {
            var cache = CacheHelper.CreateIntMRU();
            cache.Add(1, 1);
            var result = cache.Get(1).Value();
            Assert.Equal(1, result);
        }

        [Fact]
        public void LRU_MultipleAddGet_Test()
        {
            var lru = CacheHelper.CreateIntLRU();
            var values = Enumerable.Range(1, 3).ToList();
            for (var i = 0; i < values.Count; i++)
                lru.Add(i, values[i]);

            for (var i = 0; i < values.Count; i++)
            {
                var result = lru.Get(i).Value();
                Assert.Equal(values[i], result);
            }
        }

        [Fact]
        public void LRU_MultipleAddGetWithString_Test()
        {
            var cache = CacheHelper.CreateLRU<string, string>();
            MultipleAddGetWithString_Test(cache);
        }

        [Fact]
        public void MRU_MultipleAddGetWithString_Test()
        {
            var cache = CacheHelper.CreateMRU<string, string>();
            MultipleAddGetWithString_Test(cache);
        }
        
        private void MultipleAddGetWithString_Test(ICache<string, string> cache)
        {
            var values = Enumerable.Range(1, 3).ToList();
            for (var i = 0; i < values.Count; i++)
                cache.Add(i.ToString(), values[i].ToString());

            for (var i = 0; i < values.Count; i++)
            {
                var result = cache.Get(i.ToString()).IfNone("");
                Assert.Equal(values[i].ToString(), result);
            }
        }
        
        [Fact]
        public void LRU_CacheSize_Test()
        {
            var cache = CacheHelper.CreateIntLRU(3,1);
            var ways = 3;
            CacheSize_Test(cache, ways);
        }

        [Fact]
        public void MRU_CacheSize_Test()
        {
            var cache = CacheHelper.CreateIntMRU(3,1);
            var ways = 3;
            CacheSize_Test(cache, ways);
        }
        
        private void CacheSize_Test(ICache<int, int> cache, int ways)
        {
            var values = Enumerable.Range(1, 10).ToList();
            for (var i = 0; i < values.Count; i++)
                cache.Add(i, values[i]);

            var count = 0;
            for (var i = 0; i < values.Count; i++)
            {
                cache.Get(i).IfSome(_ => count++);
            }
            Assert.Equal(ways, count);
        }
    }
}