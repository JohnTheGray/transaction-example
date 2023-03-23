using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionCaches
{
    public class DeleteRowOperation : IOperation
    {
        public string RowName { get; }

        public DeleteRowOperation(string rowName)
        {
            RowName = rowName;
        }
    }
}
