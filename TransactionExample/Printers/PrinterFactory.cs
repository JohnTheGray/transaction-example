using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.TransactionCaches;

namespace TransactionExample.Printers
{
    public class PrinterFactory : IPrinterFactory
    {
        public IPrinter Create(IDataStore dataStore, ITransactionCache transactionCache)
        {
            return new Printer(dataStore, transactionCache);
        }
    }
}
