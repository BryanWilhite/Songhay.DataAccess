using System.Data;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.DataAccess.Tests;

public class SqliteConnectionTests
{
    public SqliteConnectionTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    [Theory]
    [InlineData("../../../../db/northwind.db")]
    public void ShouldOpenSqliteConnection(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"Data Source={dbPath}";

        using SqliteConnection connection = new (connectionString);
        connection.Open();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    readonly ITestOutputHelper _helper;
}
