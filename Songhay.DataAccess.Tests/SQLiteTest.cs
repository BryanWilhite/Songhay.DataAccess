using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Extensions;
using Songhay.Extensions;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Songhay.DataAccess.Tests
{

    [TestClass]
    public partial class SQLiteTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sqlite""")]
        public void ShouldConnectToChinookWithSQLiteConnection()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
            var connectionString = this.TestContext.ShouldGetConnectionString(projectsFolder);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                Assert.AreEqual(connection.State, ConnectionState.Open, "The expected Open Connection State is not here.");
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sqlite""")]
        [TestProperty("sql", "SELECT * FROM [Employee];")]
        public void ShouldFetchData()
        {
            var sql = this.TestContext.Properties["sql"].ToString();

            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
            var connectionString = this.TestContext.ShouldGetConnectionString(projectsFolder);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var xpd = CommonReaderUtility.GetXPathDocument(connection, sql, "table", "row");
                this.TestContext.WriteLine(xpd.CreateNavigator().OuterXml);
            }
        }

        [TestMethod]
        [TestProperty("chocolateyPath", @"chocolatey\lib\SQLite\tools\sqlite3.dll")]
        public void ShouldFindSQLiteDll()
        {
            var chocolateyPath = this.TestContext.Properties["chocolateyPath"].ToString();

            var systemDrive = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(systemDrive, chocolateyPath);
            this.TestContext.ShouldFindFile(path);
        }
    }
}
