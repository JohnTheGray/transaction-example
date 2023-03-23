using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionExample.DataStores;

namespace TransactionExample.Tests
{
    public class DataStoreTests
    {

        [Fact]
        public void Test_GetRows_Succeeds()
        {
            var sut = new DataStore();

            sut.SetRow("row1", new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } });

            sut.GetRow("row1").Should().BeEquivalentTo(new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } });
        }

        [Fact]
        public void Test_SetRow_Overwrite_Succeeds()
        {
            var sut = new DataStore();

            sut.SetRow("row1", new Dictionary<string, string> { { "col1", "val1" } });

            // Overwrite.
            sut.SetRow("row1", new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } });

            sut.GetRow("row1").Should().BeEquivalentTo(new Dictionary<string, string> { { "col1", "val1" }, { "col2", "val2" } });
        }

        [Fact]
        public void Test_GetRowNames_Succeeds()
        {
            var sut = new DataStore();

            sut.SetRow("row1", new Dictionary<string, string> { { "col1", "val1" } });

            sut.SetRow("row2", new Dictionary<string, string> { { "col2", "val2" } });

            sut.SetRow("row3", new Dictionary<string, string> { { "col3", "val3" } });

            sut.GetRowNames().Should().BeEquivalentTo(new List<string> { "row1", "row2", "row3" });
        }
    }
}
