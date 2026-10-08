using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;
using Songhay.DataAccess.Models;

namespace Songhay.DataAccess.Tests;

public class SqliteConnectionTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData("../../../../db/northwind.db")]
    public void ShouldOpenSqliteConnection(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"Data Source={dbPath}";

        helper.WriteLine($"{nameof(connectionString)}: {connectionString}");

        using SqliteConnection connection = new (connectionString);
        connection.Open();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Theory]
    [InlineData("../../../../db/northwind.db")]
    public void ShouldOpenSqliteConnectionWithCommonDb(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        //arrange:
        const string invariantProviderName = "Microsoft.Data.Sqlite";
        DbProviderFactories.RegisterFactory(invariantProviderName, SqliteFactory.Instance);
        string connectionString = $"Data Source={dbPath}";

        helper.WriteLine($"{nameof(connectionString)}: {connectionString}");

        //act:
        using CommonDbms commonDbms = new (invariantProviderName, connectionString, cnn=> Assert.Equal(ConnectionState.Open, cnn.State));

        //assert:
        Assert.Equal(ConnectionState.Open, commonDbms.Connection.State);
    }
}
