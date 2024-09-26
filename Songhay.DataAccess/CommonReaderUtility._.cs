using System.Collections;
using System.Data;
using System.Data.Common;

namespace Songhay.DataAccess;

/// <summary>
/// A few static helper members
/// for <see cref="DbDataReader"/>
/// and selected <c>System.Data</c> interfaces.
/// </summary>
public static partial class CommonReaderUtility
{
    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The <see cref="DbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query) => GetReaderCommand(connection, query, null);

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement or stored procedure execution.</param>
    /// <param name="parameters">A list of parameters.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, params (string Name, object? Value)[]? parameters)
    {
        IEnumerable<(string Name, object? Value)>? parameterCollection = parameters;

        return GetReaderCommand(connection, query, parameterCollection, timeout: 30, ambientTransaction: null);
    }

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, IEnumerable? parameterCollection) =>
        GetReaderCommand(connection, query, parameterCollection, timeout: 30);

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement or stored procedure execution.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    /// <param name="parameters">A list of parameters.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, int timeout, params (string Name, object? Value)[]? parameters)
    {
        IEnumerable<(string Name, object? Value)>? parameterCollection = parameters;

        return GetReaderCommand(connection, query, parameterCollection, timeout, ambientTransaction: null);
    }

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, IEnumerable? parameterCollection, int timeout) =>
        GetReaderCommand(connection, query, parameterCollection, timeout, ambientTransaction: null);

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement or stored procedure execution.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    /// <param name="ambientTransaction">The ambient <see cref="IDbTransaction"/> implementation.</param>
    /// <param name="parameters">A list of parameters.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, int timeout, IDbTransaction? ambientTransaction, params (string Name, object? Value)[]? parameters)
    {
        IEnumerable<(string Name, object? Value)>? parameterCollection = parameters;

        return GetReaderCommand(connection, query, parameterCollection, timeout, ambientTransaction);
    }

    /// <summary>
    /// Returns an instance the conventional <see cref="IDbCommand"/>
    /// for generating an <see cref="IDataReader"/>
    /// based on the specified <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement or stored procedure execution.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    /// <param name="ambientTransaction">The ambient <see cref="IDbTransaction"/> implementation.</param>
    public static IDbCommand GetReaderCommand(IDbConnection? connection, string? query, IEnumerable? parameterCollection, int timeout, IDbTransaction? ambientTransaction)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The implementing Connection object is null.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

        IDbCommand selectCommand = connection.CreateCommand();

        IReadOnlyCollection<IDataParameter> parameters = CommonParameterUtility.GetParameters(selectCommand, parameterCollection);
        selectCommand.CommandType = query.ToLower().Contains("select ") ? CommandType.Text : CommandType.StoredProcedure;
        selectCommand.CommandText = query;
        selectCommand.CommandTimeout = timeout;

        foreach (IDataParameter p in parameters)
        {
            selectCommand.Parameters.Add(p);
        }

        if (ambientTransaction != null) selectCommand.Transaction = ambientTransaction;

        return selectCommand;
    }
}