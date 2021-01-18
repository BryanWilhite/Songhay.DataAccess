using Songhay.Extensions;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace Songhay.DataAccess.Tests
{

    public partial class SQLiteTest
    {
        [Theory]
        [InlineData(@"Data Source=""../../../Chinook.sqlite""")]
        public void ShouldConnectToChinookWithSQLiteConnection(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                Assert.Equal(ConnectionState.Open, connection.State);
            }
        }

        // [Theory]
        // [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sqlite""")]
        // [TestProperty("sql", "SELECT * FROM [Employee];")]
        // public void ShouldFetchData()
        // {
        //     var sql = this.TestContext.Properties["sql"].ToString();

        //     var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
        //     var connectionString = this.TestContext.ShouldGetConnectionString(projectsFolder);
        //     using (var connection = new SQLiteConnection(connectionString))
        //     {
        //         connection.Open();

        //         var xpd = CommonReaderUtility.GetXPathDocument(connection, sql, "table", "row");
        //         this.TestContext.WriteLine(xpd.CreateNavigator().OuterXml);
        //     }
        // }

        // [Theory]
        // [TestProperty("chocolateyPath", @"chocolatey\lib\SQLite\tools\sqlite3.dll")]
        // public void ShouldFindSQLiteDll()
        // {
        //     var chocolateyPath = this.TestContext.Properties["chocolateyPath"].ToString();

        //     var systemDrive = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //     var path = Path.Combine(systemDrive, chocolateyPath);
        //     this.TestContext.ShouldFindFile(path);
        // }
    }
}
