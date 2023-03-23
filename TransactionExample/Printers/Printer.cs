using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.Printers
{
    public class Printer : IPrinter
    {
        private readonly IDataStore _dataStore;

        private readonly ITransactionCache _transactionCache;

        public Printer(
            IDataStore dataStore,
            ITransactionCache transactionCache)
        {
            _dataStore = dataStore;
            _transactionCache = transactionCache;
        }

        public void PrintAll()
        {
            // Get all rows in the cache and the backing store. These are all the rows we are concerned with printing.
            var allRowNames = _transactionCache.GetRowNames().Union(_dataStore.GetRowNames());

            Console.WriteLine("++++");
            foreach (var rowName in allRowNames)
            {
                var mergedRow = _transactionCache.GetMergedRow(rowName, (name) => _dataStore.GetRow(name));

                if (mergedRow.Count > 0)
                {
                    PrintRow(rowName, mergedRow);
                }
            }
            Console.WriteLine("----");
        }

        private static void PrintRow(string rowName, Dictionary<string, string> row)
        {
            Console.Write($"row={rowName}: ");
            Console.WriteLine(string.Join(", ", row.Select(x => $"{x.Key}={x.Value}")));
        }
    }
}
