using LanguageExt;

namespace N_WaySet_associateCache.Cache
{
    public interface ICache<TKey, TValue>
    {
        public void Add(TKey key, TValue value);
        public Option<TValue> Get(TKey key);
        public void Remove(TKey key);
        public bool Contain(TKey key);
    }
}