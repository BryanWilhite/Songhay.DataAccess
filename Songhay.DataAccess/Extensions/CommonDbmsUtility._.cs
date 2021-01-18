using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Songhay.DataAccess
{
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
        public static void Close(IDbConnection connection)
        {
            if (connection == null) return;

            if (connection.State == ConnectionState.Closed)
            {
                connection.Dispose();
                return;
            }

            if (connection.State != ConnectionState.Closed)
            {
                try
                {
                    connection.Close();
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Executes a SQL Statement for the current instance of <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns>Returns the number of records affected.</returns>
        public static int DoCommand(IDbConnection connection, string sqlStatement)
        {
            return DoCommand(connection, null, sqlStatement, null);
        }

        /// <summary>
        /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
        /// <param name="ambientTransaction">An object implementing the explicit, server <see cref="IDbTransaction"/>.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns>Returns the number of records affected.</returns>
        public static int DoCommand(IDbConnection connection, IDbTransaction ambientTransaction, string sqlStatement)
        {
            return DoCommand(connection, ambientTransaction, sqlStatement, null);
        }

        /// <summary>
        /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameterCollection">The parameters.</param>
        /// <returns>Returns the number of records affected.</returns>
        public static int DoCommand(IDbConnection connection, string sqlStatement, IEnumerable parameterCollection)
        {
            return DoCommand(connection, null, sqlStatement, parameterCollection);
        }

        /// <summary>
        /// Executes a SQL sqlStatement for the current instance of <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
        /// <param name="ambientTransaction">An instance of the explicit, server <see cref="IDbTransaction"/>.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameterCollection">The parameters.</param>
        /// <returns>Returns the number of records affected.</returns>
        public static int DoCommand(IDbConnection connection, IDbTransaction ambientTransaction, string sqlStatement, IEnumerable parameterCollection)
        {
            if (connection == null) throw new ArgumentNullException("connection", "The implementing Connection object is null.");
            if (string.IsNullOrEmpty(sqlStatement)) throw new ArgumentException("The DBMS SQL Statement was not specified.");

            int i = 0;

            using (IDbCommand cmd = connection.CreateCommand())
            {
                if (ambientTransaction != null) cmd.Transaction = ambientTransaction;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStatement;

                if (parameterCollection != null)
                {
                    IDataParameter[] paramArray = CommonParameterUtility.GetParameters(cmd, parameterCollection);
                    foreach (IDataParameter p in paramArray)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                i = cmd.ExecuteNonQuery();

            }

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
        public static DbDataAdapter GetAdapter(DbProviderFactory factory, string connectionConfiguration, string query)
        {
            if (factory == null) throw new ArgumentNullException("factory", "The expected provider factory is not here.");
            if (string.IsNullOrEmpty(connectionConfiguration)) throw new ArgumentException("The DBMS Connection string was not specified.");
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

            DbDataAdapter adapter = factory.CreateDataAdapter();

            if (string.IsNullOrEmpty(connectionConfiguration) && string.IsNullOrEmpty(query)) return adapter;

            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionConfiguration;

            DbCommand selectCommand = factory.CreateCommand();
            selectCommand.CommandText = query;
            selectCommand.Connection = connection;

            DbCommandBuilder builder = factory.CreateCommandBuilder();
            builder.DataAdapter = adapter;
            adapter.SelectCommand = selectCommand;

            adapter.DeleteCommand = builder.GetDeleteCommand();
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.UpdateCommand = builder.GetUpdateCommand();

            return adapter;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public static DbCommand GetCommand(DbProviderFactory factory, CommandType commandType, string commandText)
        {
            if (factory == null) throw new ArgumentNullException("factory", "The expected provider factory is not here.");

            DbCommand command = factory.CreateCommand();
            command.CommandType = commandType;
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
        public static DbConnection GetConnection(DbProviderFactory factory, string connectionConfiguration)
        {
            if (factory == null) throw new ArgumentNullException("factory", "The expected provider factory is not here.");
            if (string.IsNullOrEmpty(connectionConfiguration)) throw new ArgumentException("The DBMS Connection string was not specified.");

            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionConfiguration;
            return connection;
        }

        /// <summary>
        /// Removes the key value pair from connection string.
        /// </summary>
        /// <param name="connectionConfiguration">The connection configuration.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">connectionConfiguration;The expected connection string is not here.</exception>
        /// <remarks>
        /// This routine can convert, say, an OLEDB connection string for use with another provider.
        /// So <c>"Provider=ORAOLEDB.ORACLE;Data Source=MY_SOURCE;User ID=myId;Password=my!#Passw0rd"</c>
        /// can be converted to <c>Data Source=MY_SOURCE;User ID=myId;Password=my!#Passw0rd"</c>
        /// with <c>CommonDbmsUtility.RemoveKeyValuePairFromConnectionString(connectionConfiguration, "Provider")</c>.
        /// </remarks>
        public static string RemoveKeyValuePairFromConnectionString(string connectionConfiguration, string key)
        {
            if (string.IsNullOrEmpty(connectionConfiguration)) throw new ArgumentNullException("connectionConfiguration", "The expected connection string is not here.");

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionConfiguration;
            if (builder.ContainsKey(key)) builder.Remove(key);

            return builder.ConnectionString;
        }
    }
}