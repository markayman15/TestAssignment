using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignment.Repository.Interfaces
{
    public interface IGenericRepository<TKey,TValue>
    {
        ConcurrentDictionary<TKey, TValue> Get();
        bool Add(KeyValuePair<TKey,TValue> item);
        bool Remove(KeyValuePair<TKey, TValue> item);
    }
}
