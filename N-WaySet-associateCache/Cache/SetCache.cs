using System;
using System.Collections.Generic;
using LanguageExt;
using N_WaySet_associateCache.EvictionPolicy;

namespace N_WaySet_associateCache.Cache
{
    public class SetCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly IEvictionPolicy<TKey> _evictionPolicy;
        private readonly Dictionary<TKey, TValue> _vault;
        private readonly int _waysCount;
        private int _keysCount;

        public SetCache(IEvictionPolicy<TKey> evictionPolicy, int waysCount)
        {
            _waysCount = waysCount > 0
                ? waysCount
                : throw new ArgumentException("Ways count should be positive and not 0");

            _evictionPolicy = evictionPolicy ?? throw new NullReferenceException("Evince policy is null");
            _vault = new Dictionary<TKey, TValue>(waysCount);
        }

        public void Add(TKey key, TValue value)
        {
            if (_keysCount > _waysCount)
                throw new OverflowException("Set cache overflow");
            if (_vault.ContainsKey(key))
            {
                _vault[key] = value;
            }
            else
            {
                if (_keysCount == _waysCount) Evict();
                _vault.Add(key, value);
                _keysCount++;
            }

            _evictionPolicy.Add(key);
        }

        public Option<TValue> Get(TKey key)
        {
            var hit = _vault.TryGetValue(key, out var value);
            if (hit)
                _evictionPolicy.Get(key);
            return hit ? value : Option<TValue>.None;
        }

        public void Remove(TKey key)
        {
            if (Contain(key))
            {
                _vault.Remove(key);
                _evictionPolicy.Remove(key);
            }
        }

        public bool Contain(TKey key)
        {
            return _vault.ContainsKey(key);
        }

        private void Evict()
        {
            _evictionPolicy.GetKeyToEvict().IfSome(keyToEvict =>
            {
                _vault.Remove(keyToEvict);
                _evictionPolicy.Remove(keyToEvict);
                _keysCount--;
            });
        }
    }
}