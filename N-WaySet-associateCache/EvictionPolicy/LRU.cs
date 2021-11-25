using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace N_WaySet_associateCache.EvictionPolicy
{
    // ReSharper disable once InconsistentNaming
    public class LRU<TKey> : IEvictionPolicy<TKey>
    {
        private readonly Dictionary<TKey, int> _vault = new();
        public void Add(TKey key)
        {
            _vault.Keys.ToList().ForEach(vaultKey => _vault[vaultKey]++);
            _vault.TryGetValue(Key: key).Match(
                    _ => _vault[key] = 0, 
                    () => _vault.Add(key, 0));
        }

        public void Get(TKey key) { /* Empty reaction on get */ }

        public void Remove(TKey key) => _vault.Remove(key);

        public Option<TKey> GetKeyToEvict()
        {
            return !_vault.Any() 
                ? Option<TKey>.None 
                : _vault.OrderByDescending(vaultPair => vaultPair.Value).First().Key;
        }
    }
}