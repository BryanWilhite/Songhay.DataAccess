using System.Data;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDbCommand"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IDbCommandExtensions
{
    /// <summary>
    /// Withes the connection.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="connection">The connection.</param>
    /// <exception cref="System.ArgumentNullException">connection;The expected connection is not here.</exception>
    public static IDbCommand WithConnection(this IDbCommand? command, IDbConnection? connection)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (connection == null) throw new ArgumentNullException(nameof(connection), "The expected connection is not here.");
        command.Connection = connection;
        return command;
    }

    /// <summary>
    /// Sets <see cref="IDbCommand"/> with the specified <see cref="IDbTransaction"/>.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <exception cref="System.ArgumentNullException">transaction;The expected transaction is not here.</exception>
    public static IDbCommand WithAmbientTransaction(this IDbCommand? command, IDbTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (transaction == null) throw new ArgumentNullException(nameof(transaction), "The expected transaction is not here.");
        command.Transaction = transaction;
        return command;
    }
}
