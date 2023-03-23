using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.DataStores
{
    /// <summary>
    /// A simple data store that store rows and columns are a dictionary of dictionaries.
    /// </summary>
    public interface IDataStore
    {
        // Gets the rpecified row from the store.
        Dictionary<string, string> GetRow(string rowName);

        // Gets all row names in the store.
        List<string> GetRowNames();

        // Sets (overwrites) the specified row in the store.
        void SetRow(string rowName, IDictionary<string, string> row);

        // Deletes the specified row from the store.
        void DeleteRow(string rowName);
    }
}
