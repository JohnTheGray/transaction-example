using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionCaches
{
    /// <summary>
    /// Store row operations as entries in a dictionary of dictionaries.
    /// 
    /// For delete opeations, log these in a set.
    /// </summary>
    public class TransactionDictionaryCache : ITransactionCache
    {
        private readonly Dictionary<string, Dictionary<string, string>> _cache = new Dictionary<string, Dictionary<string, string>>();

        private readonly HashSet<string> _deletedRows = new HashSet<string>();

        public void AddColumn(string rowName, string colName, string val)
        {
            if (!_cache.TryGetValue(rowName, out var row))
            {
                // Doesn't exist, so create the row.
                row = new Dictionary<string, string>();
                _cache.Add(rowName, row);
            }

            row.Add(colName, val);
        }

        public void Clear()
        {
            _cache.Clear();
            _deletedRows.Clear();
        }

        public void DeleteRow(string rowName)
        {
            _cache.Remove(rowName);
            _deletedRows.Add(rowName);
        }

        /// <summary>
        /// Gets the merged row by merging any adds and deletes into the row return by the readRowFunc.
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="readRowFunc">A function that return the backing row</param>
        /// <returns>The merged row, or the backing row if no operations for the row exist</returns>
        public Dictionary<string, string> GetMergedRow(string rowName, Func<string, Dictionary<string, string>> readRowFunc)
        {
            if (_cache.TryGetValue(rowName, out var row))
            {
                if (_deletedRows.Contains(rowName))
                {
                    // Row deletion happened at some point, hence ignore backing store and just return the cached row.
                    return new Dictionary<string, string>(row);
                }

                // Merge the cache row into the backing row.
                var backingRow = readRowFunc(rowName);
                var result = new Dictionary<string, string>(backingRow);
                foreach(var (colName, value) in row)
                {
                    result.Remove(colName);
                    result.Add(colName, value);
                }

                // Return the merged result.
                return result;
            }

            // Nothing in the cache: if not deleted, then return backing row.
            return _deletedRows.Contains(rowName) ? new Dictionary<string, string>() : readRowFunc(rowName);
        }

        public List<string> GetRowNames()
        {
            return _cache.Keys.ToList();
        }
    }
}
