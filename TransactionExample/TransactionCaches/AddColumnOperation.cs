using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionCaches
{
    public class AddColumnOperation : IOperation
    {
        public string RowName { get; }

        public string ColumnName { get; }

        public string Value { get; }

        public AddColumnOperation(string rowName, string columnName, string value)
        {
            RowName = rowName;
            ColumnName = columnName;
            Value = value;
        }
    }
}
