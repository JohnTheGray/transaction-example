using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionExample.TransactionManagers
{
    /// <summary>
    /// Manges a transaction over the begin, commit and abort cycle.
    /// </summary>
    public interface ITransactionManager
    {
        void Begin();

        void Commit();

        void Abort();
    }
}
