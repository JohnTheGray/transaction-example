using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.TransactionManagers
{
    /// <summary>
    /// A factory that produces an ITransactionManager given a data store and a transaction cache.
    /// 
    /// The purpose of the factory is to simplify DI.
    /// </summary>
    public interface ITransactionManagerFactory
    {
        ITransactionManager Create(IDataStore dataStore, ITransactionCache transactionCache);
    }
}
