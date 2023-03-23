using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.TransactionManagers
{
    public class TransactionManagerFactory : ITransactionManagerFactory
    {
        public ITransactionManager Create(IDataStore dataStore, ITransactionCache transactionCache)
        {
            return new TransactionManager(dataStore, transactionCache);
        }
    }
}
