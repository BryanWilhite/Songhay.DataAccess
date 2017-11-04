using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Extensions;
using Songhay.Extensions;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Songhay.DataAccess.Tests
{

    [TestClass]
    public partial class SQLiteTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sqlite""")]
        [TestProperty("invariantProviderName", "System.Data.SQLite")]
        public void ShouldConnectToChinook()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            this.TestContext.ShouldOpenConnection(dbFolder);
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sqlite""")]
        public void ShouldConnectToChinookWithSQLiteConnection()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            var connectionString = this.TestContext.ShouldGetConnectionString(dbFolder);
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
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectory(this.GetType());
            projectsFolder = FrameworkFileUtility.GetParentDirectory(projectsFolder, 2);
            this.TestContext.ShouldFindFolder(projectsFolder);

            var sql = this.TestContext.Properties["sql"].ToString();

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            var connectionString = this.TestContext.ShouldGetConnectionString(dbFolder);
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

        [TestMethod]
        [TestProperty("invariantProviderName", "System.Data.SQLite")]
        public void ShouldFindSQLiteFactory()
        {
            var invariantProviderName = this.TestContext.Properties["invariantProviderName"].ToString();

            var rows = DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>().ToArray();
            Assert.IsTrue(rows.Any(), "The expected rows are not here.");

            var data = rows.Select(i => string.Format(
                "Name: {0}, Description: {1}, InvariantName: {2}, AssemblyQualifiedName: {3}",
                i.Field<string>(0),
                i.Field<string>(1),
                i.Field<string>(2),
                i.Field<string>(3)))
                .ToArray();

            data.ForEachInEnumerable(i => this.TestContext.WriteLine(i));

            var datum = data.FirstOrDefault(i => i.Contains(invariantProviderName));
            Assert.IsNotNull(datum, string.Format("{0} was expected.", invariantProviderName));
        }
    }
}
