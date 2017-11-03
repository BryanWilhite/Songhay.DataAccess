using System;
using System.Data;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="DbCommand"/>
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Withes the connection.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">connection;The expected connection is not here.</exception>
        public static IDbCommand WithConnection(this IDbCommand command, IDbConnection connection)
        {
            if (command == null) return null;
            if (connection == null) throw new ArgumentNullException("connection", "The expected connection is not here.");
            command.Connection = connection;
            return command;
        }

        /// <summary>
        /// Sets <see cref="IDbCommand"/> with the specified <see cref="IDbTransaction"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">transaction;The expected transaction is not here.</exception>
        public static IDbCommand WithAmbientTransaction(this IDbCommand command, IDbTransaction transaction)
        {
            if (command == null) return null;
            if (transaction == null) throw new ArgumentNullException("transaction", "The expected transaction is not here.");
            command.Transaction = transaction;
            return command;
        }
    }
}
