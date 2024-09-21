using System.Data;
using System.Data.Common;

namespace Songhay.DataAccess;

/// <summary>
/// Generic procedures for data access.
/// </summary>
public static partial class CommonDbmsUtility
{
    /// <summary>
    /// Returns a <see cref="DbDataAdapter"/>.
    /// </summary>
    /// <param name="invariantProviderName">
    /// The invariant name of the data provider.
    /// </param>
    public static DbDataAdapter? GetAdapter(string? invariantProviderName)
    {
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentException("The Invariant Provider Name was not specified.");

        DbProviderFactory factory = DbProviderFactories.GetFactory(invariantProviderName);

        return factory.CreateDataAdapter();
    }

    /// <summary>
    /// Returns a <see cref="DbDataAdapter"/>.
    /// </summary>
    /// <param name="invariantProviderName">
    /// The invariant name of the data provider.
    /// </param>
    /// <param name="connectionConfiguration">
    /// The provider connection string.
    /// </param>
    /// <param name="query">
    /// The SELECT statement used to generate SELECT, INSERT, UPDATE, DELETE
    /// <see cref="DbCommand"/> commands.
    /// </param>
    public static DbDataAdapter GetAdapter(string? invariantProviderName, string? connectionConfiguration, string? query)
    {
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentException("The Invariant Provider Name was not specified.");

        DbProviderFactory factory = GetProviderFactory(invariantProviderName);
        DbDataAdapter adapter = GetAdapter(factory, connectionConfiguration, query);

        return adapter;
    }

    /// <summary>
    /// Gets the command.
    /// </summary>
    /// <param name="invariantProviderName">Name of the invariant provider.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="commandText">The command text.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">The Invariant Provider Name was not specified.</exception>
    public static DbCommand GetCommand(string? invariantProviderName, CommandType? commandType, string? commandText)
    {
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentException("The Invariant Provider Name was not specified.");

        DbProviderFactory? factory = GetProviderFactory(invariantProviderName);
        DbCommand command = GetCommand(factory, commandType, commandText);

        return command;
    }

    /// <summary>
    /// Returns a <see cref="DbConnection"/>.
    /// </summary>
    /// <param name="invariantProviderName">
    /// The invariant name of the data provider.
    /// </param>
    /// <param name="connectionConfiguration">
    /// The provider connection string.
    /// </param>
    public static DbConnection GetConnection(string? invariantProviderName, string? connectionConfiguration)
    {
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentException("The Invariant Provider Name was not specified.");

        DbProviderFactory factory = GetProviderFactory(invariantProviderName);
        DbConnection connection = GetConnection(factory, connectionConfiguration);

        return connection;
    }

    /// <summary>
    /// Gets the provider factory.
    /// </summary>
    /// <param name="invariantProviderName">Name of the invariant provider.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">The Invariant Provider Name was not specified.</exception>
    public static DbProviderFactory GetProviderFactory(string? invariantProviderName)
    {
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentException("The Invariant Provider Name was not specified.");
        DbProviderFactory factory = DbProviderFactories.GetFactory(invariantProviderName);
        return factory;
    }
}