using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;

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
    }
}
