/*using Songhay.Tests;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.DataAccess.Tests
{

    public partial class SQLiteTests
    {
        public SQLiteTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [ProjectFileData(typeof(SQLiteTests),
            new object[] { @"Data Source=""{0}""" }, "../../../Chinook.sqlite")]
        public void ConnectionState_Test(string connectionStringTemplate, FileInfo dbInfo)
        {
            var connectionString = string.Format(connectionStringTemplate, dbInfo.FullName);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                Assert.Equal(ConnectionState.Open, connection.State);
            }
        }

        [Theory]
        [ProjectFileData(typeof(SQLiteTests),
            new object[] { @"Data Source=""{0}""", "SELECT * FROM [Employee];" },
            "../../../Chinook.sqlite")]
        public void GetXPathDocument_Test(string connectionStringTemplate, string sql, FileInfo dbInfo)
        {
            var connectionString = string.Format(connectionStringTemplate, dbInfo.FullName);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var xpd = CommonReaderUtility.GetXPathDocument(connection, sql, "table", "row");
                this._testOutputHelper.WriteLine(xpd.CreateNavigator().OuterXml);
            }
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}*/