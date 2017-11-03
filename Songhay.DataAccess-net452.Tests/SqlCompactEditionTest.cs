using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Extensions;
using Songhay.Extensions;
using System.IO;

namespace Songhay.DataAccess.Tests
{

    [TestClass]
    public class SqlCompactEditionTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Chinook.sdf""")]
        [TestProperty("invariantProviderName", "System.Data.SqlServerCe.4.0")]
        public void ShouldConnectToChinook()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            this.TestContext.ShouldOpenConnection(dbFolder);
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Northwind.sdf""")]
        [TestProperty("invariantProviderName", "System.Data.SqlServerCe.4.0")]
        public void ShouldConnectToNorthwind()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            this.TestContext.ShouldOpenConnection(dbFolder);
        }
    }
}
