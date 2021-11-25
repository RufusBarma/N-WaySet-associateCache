using LanguageExt;

namespace N_WaySet_associateCache.EvictionPolicy
{
    public class MRU<TKey> : IEvictionPolicy<TKey>
    {
        public void Add(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public void Get(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public Option<TKey> GetKeyToEvict()
        {
            throw new System.NotImplementedException();
        }
    }
}