using Songhay.DataAccess.Extensions;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Songhay.DataAccess.Models;

/// <summary>
/// Represents any DBMS with the specified
/// invariant provider name.
/// </summary>
public sealed class CommonDbms : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommonDbms"/> class.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="invariantProviderName">Name of the invariant provider.</param>
    /// <param name="connectionStringKey">The connection string key.</param>
    public CommonDbms(IConfiguration configuration, string invariantProviderName, string? connectionStringKey)
    {

        InvariantProviderName = invariantProviderName;
        ProviderFactory = CommonDbmsUtility.GetProviderFactory(InvariantProviderName);

        string connectionString = configuration.GetConnectionString(connectionStringKey);

        _connection = CommonDbmsUtility.GetConnection(ProviderFactory, connectionString);
        _connection.Open();

        OnConnectionOpen(_connection);
    }

    /// <summary>
    /// Gets the name of the invariant provider.
    /// </summary>
    /// <value>
    /// The name of the invariant provider.
    /// </value>
    public string InvariantProviderName { get; }

    /// <summary>
    /// Gets the provider factory.
    /// </summary>
    /// <value>
    /// The provider factory.
    /// </value>
    public DbProviderFactory ProviderFactory { get; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => CommonDbmsUtility.Close(_connection);

    /// <summary>
    /// Gets the provider not supported exception.
    /// </summary>
    /// <returns></returns>
    private Exception GetProviderNotSupportedException() => new NotSupportedException($"Provider `{InvariantProviderName}` is not supported.");

    /// <summary>
    /// Gets the SQL by the specified key or <see cref="CallerMemberNameAttribute"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    private string GetSql([CallerMemberName] string? key = null)
    {
        var sql = _sql[key];

        return (InvariantProviderName == CommonDbmsConstants.OdbcProvider) ?
            sql.WithOdbcStyleParameters()
            :
            sql;
    }

    /// <summary>
    /// Called when the <see cref="DbConnection"/> is open.
    /// </summary>
    /// <param name="connection">The connection.</param>
    private void OnConnectionOpen(DbConnection connection)
    {
    }

    /// <summary>
    /// Sets the SQL.
    /// </summary>
    /// <param name="sqlSetter">The SQL setter.</param>
    /// <exception cref="System.ArgumentNullException">sqlSetter;The expected SQL-statement setter is not here.</exception>
    private void SetSql(Func<Dictionary<string, string>> sqlSetter)
    {
        if (sqlSetter == null) throw new ArgumentNullException(nameof(sqlSetter), "The expected SQL-statement setter is not here.");

        _sql = sqlSetter.Invoke();
    }

    readonly DbConnection _connection;
    Dictionary<string, string> _sql;
}