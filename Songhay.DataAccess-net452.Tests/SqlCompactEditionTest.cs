using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Extensions;
using Songhay.Extensions;

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
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
            this.TestContext.ShouldOpenConnection(projectsFolder);
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("connectionString", @"Data Source=""{0}\Northwind.sdf""")]
        [TestProperty("invariantProviderName", "System.Data.SqlServerCe.4.0")]
        public void ShouldConnectToNorthwind()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
            this.TestContext.ShouldOpenConnection(projectsFolder);
        }
    }
}
