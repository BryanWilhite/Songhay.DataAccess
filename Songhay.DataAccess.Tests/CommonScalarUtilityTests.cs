using System.Data;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Songhay.DataAccess.Tests;

public class CommonScalarUtilityTests
{
    [Theory]
    [InlineData("../../../../db/northwind.db", "SELECT LastName FROM Employees WHERE EmployeeID=4", "Peacock")]
    public void GetObject_Test(string dbPath, string sql, string expectedResult)
    {
        dbPath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, dbPath);
        Assert.True(File.Exists(dbPath));

        string connectionString = $"data source={dbPath}";

        using IDbConnection connection = CommonDbmsUtility.GetConnection(SqliteFactory.Instance, connectionString);
        Assert.NotNull(connection);
        connection.Open();

        object? actual = CommonScalarUtility.GetObject(connection, sql);
        Assert.NotNull(actual);
        Assert.Equal(expectedResult, actual.ToString());
    }
}