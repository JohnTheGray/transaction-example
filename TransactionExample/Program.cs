
using TransactionExample;
using TransactionExample.DataStores;
using TransactionExample.Printers;
using TransactionExample.TransactionCaches;
using TransactionExample.TransactionManagers;

// Please see TransactionalStoreTests for a more comprehensive set of tests.

var dataStore = new DataStore();

//var transactionCache = new TransactionDictionaryCache();
var transactionCache = new TransactionOperationCache();

var transactionManagerFactory = new TransactionManagerFactory();

var printerFactory = new PrinterFactory();

var transactionalStore = new TransactionalStore(dataStore, transactionCache, transactionManagerFactory, printerFactory);

transactionalStore.BeginTransaction();

transactionalStore.AddColumn("row1", "col1", "val1");

transactionalStore.AddColumn("row2", "col1", "val1");

transactionalStore.PrintRows();

transactionalStore.DeleteRow("row1");

transactionalStore.AddColumn("row1", "col2", "val2");

transactionalStore.PrintRows();

transactionalStore.CommitTransaction();

transactionalStore.PrintRows();

transactionalStore.BeginTransaction();

transactionalStore.DeleteRow("row1");

transactionalStore.AddColumn("row1", "col1", "val1");

transactionalStore.DeleteRow("row2");

transactionalStore.PrintRows();
