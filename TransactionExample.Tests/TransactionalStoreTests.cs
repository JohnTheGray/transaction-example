using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;
using TransactionExample.Printers;
using TransactionExample.TransactionCaches;
using TransactionExample.TransactionManagers;

namespace TransactionExample.Tests
{
    public class TransactionalStoreTests
    {
        private readonly TransactionalStore _dictionaryTransactionalStore;

        private readonly TransactionalStore _operationsTransactionalStore;

        private readonly TestPrinterFactory _printerFactoryForDictionary;

        private readonly TestPrinterFactory _printerFactoryForOperations;

        public TransactionalStoreTests()
        {
            var dataStore = new DataStore();

            var transactionManagerFactory = new TransactionManagerFactory();

            _printerFactoryForDictionary = new TestPrinterFactory();

            _printerFactoryForOperations = new TestPrinterFactory();

            var transactionDictionaryCache = new TransactionDictionaryCache();

            var transactionOperationalCache = new TransactionOperationCache();

            _dictionaryTransactionalStore = new TransactionalStore(dataStore, transactionDictionaryCache, transactionManagerFactory, _printerFactoryForDictionary);

            _operationsTransactionalStore = new TransactionalStore(dataStore, transactionOperationalCache, transactionManagerFactory, _printerFactoryForOperations);
        }

        /// <summary>
        /// Return the correct store and printer test instances depending on whether we want to test with the dictionary
        /// or operation list cache implementations.
        /// </summary>
        /// <param name="isOperationListCache"></param>
        /// <returns></returns>
        private (TransactionalStore transactionalStore, TestPrinterFactory printer) GetTestInstances(bool isOperationListCache)
        {
            if (isOperationListCache)
            {
                return (_operationsTransactionalStore, _printerFactoryForOperations);
            }

            return (_dictionaryTransactionalStore, _printerFactoryForDictionary);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddSingleColumn_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            var checkExpectedData = () => printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            // Check before commit.
            checkExpectedData();

            transactionalStore.CommitTransaction();

            // Check after commit.
            checkExpectedData();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddMultiColumn_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            var checkExpectedData = () => printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            transactionalStore.AddColumn("row1", "col2", "val2");

            // Check before commit.
            checkExpectedData();

            transactionalStore.CommitTransaction();

            // Check after commit.
            checkExpectedData();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddSingleColumnToExistingRow_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col2", "val2");

            var checkExpectedData = () => printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } } }
            });

            checkExpectedData();

            transactionalStore.CommitTransaction();

            checkExpectedData();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_DeleteNonExistingRow_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.DeleteRow("row1");

            // Check before commit.
            printer.Printer!.RowData.Should().BeEmpty();

            transactionalStore.CommitTransaction();

            // Check after commit.
            printer.Printer!.RowData.Should().BeEmpty();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_DeleteExistingRow_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.DeleteRow("row1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEmpty();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddColumnToExistingRowThenDelete_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col2", "val2");

            transactionalStore.DeleteRow("row1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEmpty();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddColumnToExistingRowThenDeleteThenAddAgain_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            transactionalStore.CommitTransaction();

            printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col2", "val2");

            // This delete should remove both the col1 from backing row and col2 from the cache.
            transactionalStore.DeleteRow("row1");

            transactionalStore.AddColumn("row1", "col3", "val3");

            var checkExpectedData = () => printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                // We should only have what was added after the delete.
                {"row1", new Dictionary<string, string> { { "col3", "val3" } } }
            });

            checkExpectedData();

            transactionalStore.CommitTransaction();

            checkExpectedData();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Test_AddColumnToNonExistingThenDeleteThenAddAgain_Succeeds(bool isOperationListCache)
        {
            var (transactionalStore, printer) = GetTestInstances(isOperationListCache);

            transactionalStore.BeginTransaction();

            transactionalStore.AddColumn("row1", "col1", "val1");

            printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col1", "val1" } } }
            });

            transactionalStore.DeleteRow("row1");

            printer.Printer!.RowData.Should().BeEmpty();

            transactionalStore.AddColumn("row1", "col2", "val2");

            var checkExpectedData = () => printer.Printer!.RowData.Should().BeEquivalentTo(new Dictionary<string, Dictionary<string, string>>
            {
                {"row1", new Dictionary<string, string> { { "col2", "val2" } } }
            });

            checkExpectedData();

            transactionalStore.CommitTransaction();

            checkExpectedData();
        }
    }

    /// <summary>
    /// This test print just exposes the merged row data via a property.
    /// </summary>
    class TestPrinter : IPrinter
    {
        private readonly IDataStore _dataStore;

        private readonly ITransactionCache _transactionCache;

        public Dictionary<string, Dictionary<string, string>> RowData
        {
            get
            {
                var result = new Dictionary<string, Dictionary<string, string>>();

                var allRowNames = _transactionCache.GetRowNames().Union(_dataStore.GetRowNames());

                foreach (var rowName in allRowNames)
                {
                    var mergedRow = _transactionCache.GetMergedRow(rowName, (name) => _dataStore.GetRow(name));

                    if (mergedRow.Count > 0)
                    {
                        result.Add(rowName, mergedRow);
                    }
                }

                return result;
            }
        }

        public TestPrinter(
            IDataStore dataStore,
            ITransactionCache transactionCache)
        {
            _dataStore = dataStore;
            _transactionCache = transactionCache;
        }

        public void PrintAll()
        {
            // N/A
        }
    }

    class TestPrinterFactory : IPrinterFactory
    {
        public TestPrinter? Printer { get; private set; }

        public IPrinter Create(IDataStore dataStore, ITransactionCache transactionCache)
        {
            Printer = new TestPrinter(dataStore, transactionCache);

            return Printer;
        }
    }
}
