using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Songhay.DataAccess.Models;
using Songhay.Extensions;

namespace Songhay.DataAccess;

/// <summary>
/// Generic procedures for data access.
/// </summary>
public static partial class CommonDbmsUtility
{
    /// <summary>
    /// Closes a database connection.
    /// </summary>
    /// <param name="connection">
    /// An instance implementing <see cref="IDbConnection"/>.
    /// </param>
    public static void Close(IDbConnection? connection)
    {
        if (connection == null) return;

        if (connection.State == ConnectionState.Closed)
        {
            connection.Dispose();

            return;
        }

        if (connection.State == ConnectionState.Closed) return;

        try
        {
            connection.Close();
        }
        finally
        {
            connection.Dispose();
        }
    }

    /// <summary>
    /// Executes a SQL Statement for the current instance of <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <returns>Returns the number of records affected.</returns>
    public static int DoCommand(IDbConnection? connection, string? sqlStatement) => DoCommand(connection, null, sqlStatement, null);

    /// <summary>
    /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="ambientTransaction">An object implementing the explicit, server <see cref="IDbTransaction"/>.</param>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <returns>Returns the number of records affected.</returns>
    public static int DoCommand(IDbConnection? connection, IDbTransaction? ambientTransaction, string? sqlStatement) => DoCommand(connection, ambientTransaction, sqlStatement, null);

    /// <summary>
    /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <param name="parameterCollection">The parameters.</param>
    /// <returns>Returns the number of records affected.</returns>
    public static int DoCommand(IDbConnection? connection, string? sqlStatement, IEnumerable? parameterCollection) => DoCommand(connection, null, sqlStatement, parameterCollection);

    /// <summary>
    /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="ambientTransaction">An instance of the explicit, server <see cref="IDbTransaction"/>.</param>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <param name="parameterCollection">The parameters.</param>
    /// <returns>Returns the number of records affected.</returns>
    public static int DoCommand(IDbConnection? connection, IDbTransaction? ambientTransaction, string? sqlStatement, IEnumerable? parameterCollection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The implementing Connection object is null.");
        if (string.IsNullOrEmpty(sqlStatement)) throw new ArgumentException("The DBMS SQL Statement was not specified.");

        using IDbCommand cmd = connection.CreateCommand();

        if (ambientTransaction != null) cmd.Transaction = ambientTransaction;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = sqlStatement;

        if (parameterCollection != null)
        {
            IReadOnlyCollection<IDataParameter> paramArray = CommonParameterUtility.GetParameters(cmd, parameterCollection);
            foreach (IDataParameter p in paramArray)
            {
                cmd.Parameters.Add(p);
            }
        }

        var i = cmd.ExecuteNonQuery();

        return i;
    }

    /// <summary>
    /// Gets the adapter.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="connectionConfiguration">The connection configuration.</param>
    /// <param name="query">The query.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">factory;The expected provider factory is not here.</exception>
    /// <exception cref="System.ArgumentException">
    /// The DBMS Connection string was not specified.
    /// or
    /// The DBMS query was not specified.
    /// </exception>
    public static IDbDataAdapter GetAdapter(DbProviderFactory? factory, string? connectionConfiguration, string? query)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory), "The expected provider factory is not here.");
        if (string.IsNullOrEmpty(connectionConfiguration)) throw new ArgumentException("The DBMS Connection string was not specified.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

        IDbDataAdapter adapter = factory.CreateDataAdapter().ToReferenceTypeValueOrThrow();

        if (string.IsNullOrEmpty(connectionConfiguration) && string.IsNullOrEmpty(query)) return adapter;

        IDbConnection connection = factory.CreateConnection().ToReferenceTypeValueOrThrow();
        connection.ConnectionString = connectionConfiguration;

        IDbCommand selectCommand = factory.CreateCommand().ToReferenceTypeValueOrThrow();
        selectCommand.CommandText = query;
        selectCommand.Connection = connection;

        DbCommandBuilder builder = factory.CreateCommandBuilder().ToReferenceTypeValueOrThrow();
        builder.DataAdapter = adapter as DbDataAdapter;
        adapter.SelectCommand = selectCommand;

        adapter.DeleteCommand = builder.GetDeleteCommand();
        adapter.InsertCommand = builder.GetInsertCommand();
        adapter.UpdateCommand = builder.GetUpdateCommand();

        return adapter;
    }

    /// <summary>
    /// Gets the <see cref="IDbCommand"/> of <see cref="CommandType.Text"/>
    /// with the specified command text.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="commandText">The command text.</param>
    public static IDbCommand GetCommand(DbProviderFactory? factory, string? commandText) => GetCommand(factory, null, commandText);

    /// <summary>
    /// Gets the <see cref="IDbCommand"/>
    /// with the specified command text.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="commandType">Type of the command (defaults to <see cref="CommandType.Text"/>).</param>
    /// <param name="commandText">The command text.</param>
    public static IDbCommand GetCommand(DbProviderFactory? factory, CommandType? commandType, string? commandText)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory), "The expected provider factory is not here.");

        IDbCommand command = factory.CreateCommand().ToReferenceTypeValueOrThrow();
        command.CommandType = commandType ?? CommandType.Text;
        command.CommandText = commandText;

        return command;
    }

    /// <summary>
    /// Gets the <see cref="IDbCommand"/>
    /// with the specified command text.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="commandType">Type of the command (defaults to <see cref="CommandType.Text"/>).</param>
    /// <param name="commandText">The command text.</param>
    public static IDbCommand GetCommand(IDbConnection? connection, CommandType? commandType, string? commandText)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The implementing Connection object is null.");

        IDbCommand command = connection.CreateCommand().ToReferenceTypeValueOrThrow();
        command.CommandType = commandType ?? CommandType.Text;
        command.CommandText = commandText;

        return command;
    }

    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="connectionConfiguration">The connection configuration.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">factory;The expected provider factory is not here.</exception>
    public static IDbConnection GetConnection(DbProviderFactory? factory, string? connectionConfiguration)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory), "The expected provider factory is not here.");
        if (string.IsNullOrEmpty(connectionConfiguration)) throw new ArgumentException("The DBMS Connection string was not specified.");

        IDbConnection connection = factory.CreateConnection().ToReferenceTypeValueOrThrow();
        connection.ConnectionString = connectionConfiguration;

        return connection;
    }

    /// <summary>
    /// Registers <see cref="SqliteFactory"/> with the current app domain.
    /// </summary>
    public static void RegisterMicrosoftSqlite() => DbProviderFactories.RegisterFactory(CommonDbmsConstants.MicrosoftSqliteProvider, SqliteFactory.Instance);

    /// <summary>
    /// Removes the key value pair from connection string.
    /// </summary>
    /// <param name="connectionString">The connection configuration.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">connectionString;The expected connection string is not here.</exception>
    /// <remarks>
    /// This routine can convert, say, an OLEDB connection string for use with another provider.
    /// So <c>"provider=ORAOLEDB.ORACLE;data source=MY_SOURCE;user id=myId;password=my!#Passw0rd"</c>
    /// can be converted to <c>data source=MY_SOURCE;user id=myId;password=my!#Passw0rd"</c>
    /// with <c>CommonDbmsUtility.RemoveKeyValuePairFromConnectionString(connectionString, "Provider")</c>.
    /// </remarks>
    public static string RemoveKeyValuePairFromConnectionString(string? connectionString, string? key)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString), "The expected connection string is not here.");

        var builder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString,
        };

        if (!string.IsNullOrWhiteSpace(key) && builder.ContainsKey(key)) builder.Remove(key);

        return builder.ConnectionString;
    }
}