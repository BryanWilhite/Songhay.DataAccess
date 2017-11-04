using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Domain.Repository;
using Songhay.Extensions;
using System;
using System.Data;

namespace Songhay.DataAccess.Tests
{
    public partial class SQLiteTest
    {
        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldConnectToChinookWithEF()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
            AppDomain.CurrentDomain.SetData("DataDirectory", projectsFolder);

            using (var context = new ChinookDbContext())
            {
                context.Database.Connection.Open();
                Assert.AreEqual(ConnectionState.Open, context.Database.Connection.State);
            }

        }
    }
}
