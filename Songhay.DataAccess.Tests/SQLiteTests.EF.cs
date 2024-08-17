/*using Microsoft.EntityFrameworkCore;
using Songhay.DataAccess.Tests.Domain.Repository;
using System;
using System.Data;
using Xunit;

namespace Songhay.DataAccess.Tests
{
    public partial class SQLiteTests
    {
        [Theory]
        [InlineData("Data Source=|DataDirectory|Chinook.sqlite")]
        //[InlineData("Name=Chinook")] TODO: System.InvalidOperationException : A named connection string was used, but the name 'Chinook' was not found in the application's configuration. Note that named connection strings are only supported when using 'IConfiguration' and a service provider, such as in a typical ASP.NET Core application. See https://go.microsoft.com/fwlink/?linkid=850912 for more information.
        public void ConnectionState_EF_Test(string connectionString)
        {
            var projectsFolder = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");
            Assert.EndsWith(".Tests", projectsFolder);

            AppDomain.CurrentDomain.SetData("DataDirectory", projectsFolder);

            var builder = new DbContextOptionsBuilder<ChinookDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseSqlite(connectionString);

            using (var context = new ChinookDbContext(builder.Options))
            {
                var connection = context.Database.GetDbConnection();
                connection.Open();
                Assert.Equal(ConnectionState.Open, connection.State);
            }

        }
    }
}*/