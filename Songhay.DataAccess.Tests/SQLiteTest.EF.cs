using Songhay.DataAccess.Tests.Domain.Repository;
using Songhay.Extensions;
using System;
using System.Data;

namespace Songhay.DataAccess.Tests
{
    public partial class SQLiteTest
    {
        // [TestCategory("Integration")]
        // [TestMethod]
        // public void ShouldConnectToChinookWithEF()
        // {
        //     var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);
        //     AppDomain.CurrentDomain.SetData("DataDirectory", projectsFolder);

        //     var builder = new DbContextOptionsBuilder<ChinookDbContext>()
        //         .UseSqlite("Name=Chinook");

        //     using (var context = new ChinookDbContext(builder.Options))
        //     {
        //         var connection = context.Database.GetDbConnection();
        //         connection.Open();
        //         Assert.AreEqual(ConnectionState.Open, connection.State);
        //     }

        // }
    }
}
