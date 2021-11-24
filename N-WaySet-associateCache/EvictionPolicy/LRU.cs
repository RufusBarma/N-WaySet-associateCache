namespace N_WaySet_associateCache.EvictionPolicy
{
    public class LRU<TKey> : IEvictionPolicy<TKey>
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

        public TKey GetKeyToEvict()
        {
            throw new System.NotImplementedException();
        }
    }
}