using System;
using System.Collections.Generic;
using LanguageExt;
using N_WaySet_associateCache.EvictionPolicy;

namespace N_WaySet_associateCache.Cache
{
    public class NWaySetCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly List<SetCache<TKey, TValue>> _sets;
        private readonly int _setsCount;

        public NWaySetCache(IEvictionPolicy<TKey> evictionPolicy, int waysCount, int setsCount)
        {
            if (waysCount < 1)
                throw new ArgumentException("Ways count should be positive and not 0");
            if (setsCount < 1)
                throw new ArgumentException("Sets count should be positive and not 0");
            if (evictionPolicy == null)
                throw new NullReferenceException("Evince policy is null");
            _setsCount = setsCount;

            _sets = new List<SetCache<TKey, TValue>>(_setsCount);
            for (var i = 0; i < _setsCount; i++)
                _sets.Add(new SetCache<TKey, TValue>(evictionPolicy, waysCount));
        }

        public void Add(TKey key, TValue value) => GetSet(key).Add(key, value);

        public Option<TValue> Get(TKey key) => GetSet(key).Get(key);

        public void Remove(TKey key) => GetSet(key).Remove(key);

        public bool Contain(TKey key) => GetSet(key).Contain(key);

        private SetCache<TKey, TValue> GetSet(TKey key) => _sets[Math.Abs(key.GetHashCode() % _setsCount)];
    }
}