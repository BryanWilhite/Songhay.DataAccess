using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.DataAccess.Tests.Domain.Repository;
using Songhay.Extensions;
using System;
using System.Data;
using System.IO;

namespace Songhay.DataAccess.Tests
{
    public partial class SQLiteTest
    {
        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldConnectToChinookWithEF()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType());

            var dbFolder = Path.Combine(projectsFolder, this.GetType().Namespace);
            this.TestContext.ShouldFindFolder(dbFolder);

            AppDomain.CurrentDomain.SetData("DataDirectory", dbFolder);

            using(var context = new ChinookDbContext())
            {
                context.Database.Connection.Open();
                Assert.AreEqual(ConnectionState.Open, context.Database.Connection.State);
            }

        }
    }
}
