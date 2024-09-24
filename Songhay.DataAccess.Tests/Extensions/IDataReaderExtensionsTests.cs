using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Songhay.DataAccess.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.DataAccess.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IDataReaderExtensionsTests
{
    public IDataReaderExtensionsTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    [Theory]
    [InlineData("../../../../db/northwind.db", "SELECT * FROM Employees")]
    public void ToJsonObject_Test(string dbPath, string sql)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using IDbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        using IDbCommand command = CommonReaderUtility.GetReaderCommand(connection, sql);
        using IDataReader reader = command.ExecuteReader();

        JsonObject? actual = reader.ToJsonObject();
        Assert.NotNull(actual);

        _helper.WriteLine(actual.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    [Theory]
    [InlineData("../../../../db/northwind.db", "SELECT * FROM Employees")]
    public void ToXDocument_Test(string dbPath, string sql)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using IDbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        using IDbCommand command = CommonReaderUtility.GetReaderCommand(connection, sql);
        using IDataReader reader = command.ExecuteReader();

        XDocument actual = reader.ToXDocument();
        Assert.NotNull(actual);

        _helper.WriteLine(actual.ToString());
    }

    readonly ITestOutputHelper _helper;
}
