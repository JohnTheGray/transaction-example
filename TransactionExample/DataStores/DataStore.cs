using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.DataStores
{
    public class DataStore : IDataStore
    {
        private readonly Dictionary<string, Dictionary<string, string>> _cache = new Dictionary<string, Dictionary<string, string>>();

        public void DeleteRow(string rowName)
        {
            _cache.Remove(rowName);
        }

        public List<string> GetRowNames()
        {
            return _cache.Keys.ToList();
        }

        public Dictionary<string, string> GetRow(string rowName)
        {
            if (_cache.TryGetValue(rowName, out var row))
            {
                return new Dictionary<string, string>(row);
            }

            // Row doesn't exist.
            return new Dictionary<string, string>();
        }

        public void SetRow(string rowName, IDictionary<string, string> row)
        {
            if (row.Count == 0)
            {
                throw new ArgumentException("cannot write empty rows, use DeleteRow instead");
            }

            // Just overwrite the existing entry.
            _cache[rowName] = new Dictionary<string, string>(row);
        }
    }
}
