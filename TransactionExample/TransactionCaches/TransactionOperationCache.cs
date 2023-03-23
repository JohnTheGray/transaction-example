using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionCaches
{
    /// <summary>
    /// Track changes in the transaction as a list of operations for each row.
    /// 
    /// To get the merged row, simply apply all operations for a given row in order.
    /// 
    /// Note that if we received a DeleteRowOperation, we *could*  for efficiency just truncate
    /// the operations list and add the DeleteRowOperation since all prior operations on the row have no effect.
    /// 
    /// Note that if we stack up multiple add operations on the same row, for efficiency we *could* merge this
    /// into a single operation for the row. Effectively, any set of operations would be represented by an optional
    /// DeleteRowOperation, followed by an optional AddMultiColumnOperation. This reduces down to something similar
    /// to the TransactionDictionaryCache.
    /// 
    /// </summary>
    public class TransactionOperationCache : ITransactionCache
    {
        // Map of rowName to operation list.
        private readonly Dictionary<string, List<IOperation>> _operationsMap = new Dictionary<string, List<IOperation>>();

        public void AddColumn(string rowName, string colName, string val)
        {
            if (!_operationsMap.TryGetValue(rowName, out var operations))
            {
                operations = new List<IOperation>();

                _operationsMap[rowName] = operations;
            }

            operations.Add(new AddColumnOperation(rowName, colName, val));
        }

        public void Clear()
        {
            _operationsMap.Clear();
        }

        public void DeleteRow(string rowName)
        {
            if (!_operationsMap.TryGetValue(rowName, out var operations))
            {
                operations = new List<IOperation>();

                _operationsMap[rowName] = operations;
            }

            operations.Add(new DeleteRowOperation(rowName));
        }

        public Dictionary<string, string> GetMergedRow(string rowName, Func<string, Dictionary<string, string>> readRowFunc)
        {
            // Start with the backing row.
            var row = readRowFunc(rowName);

            if (_operationsMap.TryGetValue(rowName, out var operations))
            {
                foreach(var operation in operations)
                {
                    switch (operation)
                    {
                        case AddColumnOperation op:
                            // Add the column into the row.
                            row.Remove(op.ColumnName);
                            row.Add(op.ColumnName, op.Value);
                            break;
                        case DeleteRowOperation:
                            // Current row/backing row is deleted, hence empty the row.
                            row = new Dictionary<string, string>();
                            break;
                        default:
                            throw new InvalidOperationException($"unknown operation type: {operation.GetType()}");
                    }
                }
            }

            return row;
        }

        public List<string> GetRowNames()
        {
            return _operationsMap.Keys.ToList();
        }
    }
}
