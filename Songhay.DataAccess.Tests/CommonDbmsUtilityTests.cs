using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.DataAccess.Tests;

public class CommonDbmsUtilityTests
{
    public CommonDbmsUtilityTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    [Theory]
    [InlineData("../../../../db/northwind.db")]
    public void GetConnection_Test(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"Data Source={dbPath}";

        using DbConnection? connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    readonly ITestOutputHelper _helper;
}
