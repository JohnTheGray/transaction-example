using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.Printers
{
    /// <summary>
    /// A factory that produces an IPrinter given a data store and a transaction cache.
    /// 
    /// The purpose of the factory is to simplify DI.
    /// </summary>
    public interface IPrinterFactory
    {
        IPrinter Create(IDataStore dataStore, ITransactionCache transactionCache);
    }
}
