using System.Data;
using System.Text.Json.Nodes;
using Microsoft.Data.Sqlite;
using Songhay.DataAccess.Extensions;
using Xunit;

namespace Songhay.DataAccess.Tests;

// ReSharper disable once InconsistentNaming
public class IDataReaderExtensionsTests
{
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

        using IDataReader reader = CommonReaderUtility.GetReader(connection, sql);

        JsonObject? actual = reader.ToJsonObject();
        Assert.NotNull(actual);
    }
}
