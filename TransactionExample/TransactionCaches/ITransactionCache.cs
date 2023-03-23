using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionCaches
{
    /// <summary>
    /// Maintains a cache of changes to rows.
    /// </summary>
    public interface ITransactionCache
    {
        void AddColumn(string rowName, string colName, string val);

        void DeleteRow(string rowName);

        void Clear();

        // Gets a merged row by applying the cached row changes to the backing row.
        Dictionary<string, string> GetMergedRow(string rowName, Func<string, Dictionary<string, string>> readRowFunc);

        // Gets all row names in the cache.
        List<string> GetRowNames();
    }
}
