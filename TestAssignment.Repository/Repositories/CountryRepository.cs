using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignment.Repository.Repositories
{
    public class CountryRepository:GenericRepository<string,string>
    {
        public CountryRepository(ConcurrentDictionary<string, string> dictionary) :base(dictionary) { }

        public IEnumerable<KeyValuePair<string,string>> GetWithSpec(int? Page,int? PageSize, string CountryCode)
        {
            if(PageSize == null && Page == null)
            {
                return _dictionary.Where(c => c.Key == CountryCode).ToList();
            }
            var result = _dictionary.Skip((Page.Value - 1) * PageSize.Value).Take(PageSize.Value).Where(c => c.Key == CountryCode);
            return result;
        }

        public bool CheckAvailability(string countryCode)
        {
            return _dictionary.ContainsKey(countryCode);
        }
    }
}
