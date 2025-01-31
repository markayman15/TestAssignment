using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignment.Repository.Interfaces;

namespace TestAssignment.Repository.Repositories
{
    public class GenericRepository<TKey, TValue> : IGenericRepository<TKey, TValue>
    {
        public readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        public GenericRepository(ConcurrentDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public bool Add(KeyValuePair<TKey, TValue> item)
        {
            var check = _dictionary.TryAdd(item.Key, item.Value);
            return check;
        }

        public ConcurrentDictionary<TKey, TValue> Get()
        {
            return _dictionary;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var check = _dictionary.TryRemove(item);
            return check;
        }
    }
}
