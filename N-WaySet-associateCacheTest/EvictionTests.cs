using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;
using N_WaySet_associateCache.EvictionPolicy;
using Xunit;

namespace N_WaySet_associateCacheTest
{
    public class MRUCacheTests
    {
        [Fact]
        public void MRU_Eviction_Test()
        {
            var eviction = new MRU<int>();
            var keys = new List<int> { 1, 2, 3 };
            keys.ForEach(key => eviction.Add(key));
            eviction.Remove(eviction.GetKeyToEvict().Value());
            eviction.Add(4);
            var result = new List<int> { 4, 2, 1 };
            Assert.True(result.All(key =>
            {
                var keyToEvict = eviction.GetKeyToEvict();
                eviction.Remove(keyToEvict.Value());
                return keyToEvict == key;
            }));
        }
        
        [Fact]
        public void MRU_EvictionRepeat_Test()
        {
            var eviction = new MRU<int>();
            var keys = new List<int> { 1, 2, 3, 1 };
            keys.ForEach(key => eviction.Add(key));
            eviction.Remove(eviction.GetKeyToEvict().Value());
            eviction.Add(4);
            var result = new List<int> { 4, 3, 2 };
            Assert.True(result.All(key =>
            {
                var keyToEvict = eviction.GetKeyToEvict();
                eviction.Remove(keyToEvict.Value());
                return keyToEvict == key;
            }));
        }
        
        [Fact]
        public void LRU_Eviction_Test()
        {
            var eviction = new LRU<int>();
            var keys = new List<int> { 1, 2, 3 };
            keys.ForEach(key => eviction.Add(key));
            Assert.Equal(1, eviction.GetKeyToEvict());
            eviction.Remove(eviction.GetKeyToEvict().Value());
            eviction.Add(4);
            var result = new List<int> { 2, 3, 4 };
            Assert.True(result.All(key =>
            {
                var keyToEvict = eviction.GetKeyToEvict();
                eviction.Remove(keyToEvict.Value());
                return keyToEvict == key;
            }));
        }
        
        [Fact]
        public void LRU_EvictionRepeat_Test()
        {
            var eviction = new LRU<int>();
            var keys = new List<int> { 1, 2, 3, 1 };
            keys.ForEach(key => eviction.Add(key));
            Assert.Equal(2, eviction.GetKeyToEvict());
            eviction.Remove(eviction.GetKeyToEvict().Value());
            eviction.Add(4);
            var result = new List<int> { 3, 1 };
            Assert.True(result.All(key =>
            {
                var keyToEvict = eviction.GetKeyToEvict();
                eviction.Remove(keyToEvict.Value());
                return keyToEvict == key;
            }));
        }
        
        [Fact]
        public void ZeroAdd_Test()
        {
            Assert.True(new MRU<int>().GetKeyToEvict().IsNone);
            Assert.True(new MRU<int>().GetKeyToEvict().IsNone);
        }
        
        [Fact]
        public void OneAdd_Test()
        {
            var evictions = new List<IEvictionPolicy<int>>
            {
                new LRU<int>(), new MRU<int>()
            };
            evictions.ForEach(eviction =>
            {
                eviction.Add(1);
                Assert.True(eviction.GetKeyToEvict().IsSome);
            });
        }
        
        [Fact]
        public void OneKeyAddMultipleTimes_Test()
        {
            var evictions = new List<IEvictionPolicy<int>>
            {
                new LRU<int>(), new MRU<int>()
            };
            evictions.ForEach(eviction =>
            {
                for(var i = 0; i < 1000; i++)
                    eviction.Add(1);
                Assert.True(eviction.GetKeyToEvict().IsSome);
                eviction.Remove(1);
                Assert.True(eviction.GetKeyToEvict().IsNone);
            });
        }
    }
}