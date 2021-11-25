using LanguageExt;

namespace N_WaySet_associateCache.EvictionPolicy
{
    public interface IEvictionPolicy<TKey>
    {
        public void Add(TKey key);
        public void Get(TKey key);
        public void Remove(TKey key);
        public Option<TKey> GetKeyToEvict();
    }
}