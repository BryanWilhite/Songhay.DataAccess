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
    /// <param name="connectionOpenHandler">the action to take when the database connection opens</param>
    public CommonDbms(IConfiguration configuration, string invariantProviderName, string? connectionStringKey, Action<IDbConnection>? connectionOpenHandler = null)
    {
        connectionStringKey.ThrowWhenNullOrWhiteSpace();

        InvariantProviderName = invariantProviderName;
        ProviderFactory = CommonDbmsUtility.GetProviderFactory(InvariantProviderName);

        string? connectionString = configuration.GetConnectionString(connectionStringKey);

        _connectionOpenHandler = connectionOpenHandler;

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
    /// Called when the <see cref="DbConnection"/> is open.
    /// </summary>
    /// <param name="connection">The connection.</param>
    private void OnConnectionOpen(IDbConnection connection) => _connectionOpenHandler?.Invoke(connection);

    private readonly IDbConnection _connection;
    private readonly Action<IDbConnection>? _connectionOpenHandler;
}
