using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Songhay.DataAccess.Models;
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
    [InlineData("../../../../db/northwind.db", "UPDATE Categories SET CategoryName = CategoryName", 8)]
    public void DoCommand_Test(string dbPath, string sql, int expected)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using DbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        connection.Open();
        var actual = CommonDbmsUtility.DoCommand(connection, sql);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("../../../../db/northwind.db", "UPDATE Categories SET CategoryName = CategoryName", 8)]
    public void DoCommand_with_DbProviderFactory_Test(string dbPath, string sql, int expected)
    {
        CommonDbmsUtility.RegisterMicrosoftSqlite();
        DbProviderFactory provider = DbProviderFactories.GetFactory(CommonDbmsConstants.MicrosoftSqliteProvider);
        Assert.NotNull(provider);

        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using DbConnection connection = CommonDbmsUtility.GetConnection(provider, connectionString);
        connection.Open();
        var actual = CommonDbmsUtility.DoCommand(connection, sql);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("../../../../db/northwind.db", "SELECT COUNT(CategoryName) FROM Categories", 8)]
    public void GetCommand_Test(string dbPath, string sql, int expected)
    {
        CommonDbmsUtility.RegisterMicrosoftSqlite();
        DbProviderFactory provider = DbProviderFactories.GetFactory(CommonDbmsConstants.MicrosoftSqliteProvider);
        Assert.NotNull(provider);

        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";
        using DbConnection? connection = provider.CreateConnection();
        Assert.NotNull(connection);

        connection.ConnectionString = connectionString;
        connection.Open();

        using var command = CommonDbmsUtility.GetCommand(provider, sql);
        command.Connection = connection;

        var actual = command.ExecuteScalar();
        Assert.NotNull(actual);
        Assert.Equal(expected, (long)actual);
    }

    [Theory]
    [InlineData("../../../../db/northwind.db")]
    public void GetConnection_Test(string dbPath)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using DbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Fact]
    public void RegisterMicrosoftSqlite_Test()
    {
        CommonDbmsUtility.RegisterMicrosoftSqlite();

        var names = DbProviderFactories.GetProviderInvariantNames().ToArray();
        Assert.NotEmpty(names);

        foreach (var name in names)
        {
            _helper.WriteLine(name);
        }
    }

    [Theory]
    [InlineData(
        "provider=ORAOLEDB.ORACLE;data source=MY_SOURCE;user id=myId;password=my!#Passw0rd",
        "provider",
        "data source=MY_SOURCE;user id=myId;password=my!#Passw0rd")]
    public void RemoveKeyValuePairFromConnectionString_Test(string connectionString, string key, string expected)
    {
        var actual = CommonDbmsUtility.RemoveKeyValuePairFromConnectionString(connectionString, key);
        Assert.Equal(expected, actual);
    }

    readonly ITestOutputHelper _helper;
}
