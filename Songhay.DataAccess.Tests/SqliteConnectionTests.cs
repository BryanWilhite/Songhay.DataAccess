using System.Data;
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
    public void ShouldOpenSqliteConnectionWithCommonDbms(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));
        CommonDbmsUtility.RegisterMicrosoftSqlite();

        //arrange:
        string connectionString = $"Data Source={dbPath}";

        helper.WriteLine($"{nameof(connectionString)}: {connectionString}");

        //act:
        using CommonDbms commonDbms = new (
            CommonDbmsConstants.MicrosoftSqliteProvider,
            connectionString,
            cnn=> Assert.Equal(ConnectionState.Open, cnn.State));

        //assert:
        Assert.Equal(ConnectionState.Open, commonDbms.Connection.State);
    }
}
