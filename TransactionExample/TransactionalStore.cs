using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.Printers;
using TransactionExample.TransactionCaches;
using TransactionExample.TransactionManagers;

namespace TransactionExample
{
    public class TransactionalStore
    {
        // Manages transaction state and performs commit.
        private readonly ITransactionManager _transactionManager;

        // A cache of operations on each row in a transaction.
        private readonly ITransactionCache _transactionCache;

        // Prints the current state of the transactional store.
        private readonly IPrinter _printer;

        public TransactionalStore(
            IDataStore dataStore,
            ITransactionCache transactionCache,
            ITransactionManagerFactory transactionManagerFactory,
            IPrinterFactory printerFactory)
        {
            _transactionCache = transactionCache;
            _transactionManager = transactionManagerFactory.Create(dataStore, _transactionCache);
            _printer = printerFactory.Create(dataStore, _transactionCache);
        }

        public void BeginTransaction()
        {
            _transactionManager.Begin();
        }

        public void CommitTransaction()
        {
            _transactionManager.Commit();
        }

        public void AbortTransaction()
        {
            _transactionManager.Abort();
        }

        public void AddColumn(string rowName, string colName, string value)
        {
            _transactionCache.AddColumn(rowName, colName, value);
        }

        public void DeleteRow(string rowName)
        {
            _transactionCache.DeleteRow(rowName);
        }

        public void PrintRows()
        {
            _printer.PrintAll();
        }
    }
}
