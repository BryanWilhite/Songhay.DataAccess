using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
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
        _loggerProvider = new XUnitLoggerProvider(helper);
    }

    [Theory]
    [InlineData("../../../../db/northwind.db", "SELECT * FROM Employees", "../../../csv", "employees")]
    public void StreamToCsvFile_Test(string dbPath, string sql, string csvPath, string outputName)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(StreamToCsvFile_Test));

        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        csvPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, $"{csvPath}/{nameof(StreamToCsvFile_Test)}/{outputName}-output.csv");

        using IDbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        using IDbCommand command = CommonReaderUtility.GetReaderCommand(connection, sql);
        using IDataReader reader = command.ExecuteReader();

        reader.StreamToCsvFile(csvPath, includeHeader: true, logger);
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
    readonly XUnitLoggerProvider _loggerProvider;
}
