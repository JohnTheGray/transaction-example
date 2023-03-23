using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.TransactionManagers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly IDataStore _dataStore;

        private readonly ITransactionCache _transactionCache;

        private bool _isTransactionStarted;

        public TransactionManager(
            IDataStore dataStore,
            ITransactionCache transactionCache)
        {
            _dataStore = dataStore;
            _transactionCache = transactionCache;
        }

        public void Abort()
        {
            if (!_isTransactionStarted)
            {
                throw new InvalidOperationException("no transaction available");
            }

            _isTransactionStarted = false;
            _transactionCache.Clear();
        }

        public void Begin()
        {
            if (_isTransactionStarted)
            {
                throw new InvalidOperationException("transaction has already started");
            }

            _isTransactionStarted = true;
        }

        public void Commit()
        {
            if (!_isTransactionStarted)
            {
                throw new InvalidOperationException("no transaction available");
            }

            // For each for row that has a cache entry ...
            foreach (var rowName in _transactionCache.GetRowNames())
            {
                var mergedRow = _transactionCache.GetMergedRow(rowName, (name) => _dataStore.GetRow(name));

                if (mergedRow.Count > 0)
                {
                    _dataStore.SetRow(rowName, mergedRow);
                }
                else
                {
                    _dataStore.DeleteRow(rowName);
                }
            }

            _isTransactionStarted = false;
        }
    }
}
