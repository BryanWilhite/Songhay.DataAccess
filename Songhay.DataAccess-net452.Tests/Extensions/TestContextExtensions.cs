using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System.Data;

namespace Songhay.DataAccess.Tests.Extensions
{
    public static class TestContextExtensions
    {
        public static string ShouldGetConnectionString(this TestContext context, string dbFolder)
        {
            context.ShouldFindFolder(dbFolder);
            var connectionString = context.Properties["connectionString"].ToString();
            connectionString = string.Format(connectionString, dbFolder);
            context.WriteLine("connecting to {0}...", connectionString);

            return connectionString;
        }

        public static void ShouldOpenConnection(this TestContext context, string dbFolder)
        {
            var invariantProviderName = context.Properties["invariantProviderName"].ToString();

            var connectionString = context.ShouldGetConnectionString(dbFolder);

            using (var connection = CommonDbmsUtility.GetConnection(invariantProviderName, connectionString))
            {
                connection.Open();
                Assert.AreEqual(connection.State, ConnectionState.Open, "The expected Open Connection State is not here.");
            }
        }
    }
}
