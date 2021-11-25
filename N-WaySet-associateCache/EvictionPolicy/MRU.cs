using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace N_WaySet_associateCache.EvictionPolicy
{
    // ReSharper disable once InconsistentNaming
    public class MRU<TKey> : IEvictionPolicy<TKey>
    {
        private readonly Dictionary<TKey, LinkedListNode<TKey>> _nodes = new();
        private readonly LinkedList<TKey> _vault = new();

        public void Add(TKey key)
        {
            GetOrDo(key,
                () =>
                {
                    var newNode = new LinkedListNode<TKey>(key);
                    _nodes.Add(key, newNode);
                    return newNode;
                });
        }

        public void Get(TKey key)
        {
            GetOrDo(key, () => throw new ArgumentException("You are trying to get not added key"));
        }

        public void Remove(TKey key)
        {
            _nodes.TryGetValue(Key: key).Match(
                node =>
                {
                    _vault.Remove(node);
                    _nodes.Remove(key);
                },
                () => throw new ArgumentException("You are trying to remove not added key"));
        }

        public Option<TKey> GetKeyToEvict()
        {
            return !_vault.Any() ? Option<TKey>.None : _vault.First.Value;
        }

        private void GetOrDo(TKey key, Func<LinkedListNode<TKey>> onDoesntExist)
        {
            var node = _nodes.TryGetValue(Key: key).Match(
                existNode =>
                {
                    _vault.Remove(existNode);
                    return existNode;
                },
                onDoesntExist);
            _vault.AddFirst(node);
        }
    }
}