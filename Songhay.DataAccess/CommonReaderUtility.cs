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
    /// Returns an instance of <see cref="IDataReader"/>
    /// based on the instance of <see cref="DbConnection"/>.
    /// </summary>
    /// <param name="connection">The <see cref="DbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    public static IDataReader GetReader(IDbConnection? connection, string? query) => GetReader(connection, query, null);

    /// <summary>
    /// Returns an instance of <see cref="System.Data.IDataReader"/>
    /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    public static IDataReader GetReader(IDbConnection? connection, string? query, IEnumerable? parameterCollection) =>
        GetReader(connection, query, parameterCollection, 30);

    /// <summary>
    /// Returns an instance of <see cref="System.Data.IDataReader"/>
    /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    public static IDataReader GetReader(IDbConnection? connection, string? query, IEnumerable? parameterCollection, int timeout) =>
        GetReader(connection, query, parameterCollection, timeout, null);

    /// <summary>
    /// Returns an instance of <see cref="System.Data.IDataReader"/>
    /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">A list of parameters.</param>
    /// <param name="timeout">Command timeout in seconds.</param>
    /// <param name="ambientTransaction">The ambient <see cref="IDbTransaction"/> implementation.</param>
    public static IDataReader GetReader(IDbConnection? connection, string? query, IEnumerable? parameterCollection, int timeout, IDbTransaction? ambientTransaction)
    {
        if (connection == null) throw new ArgumentNullException("connection", "The implementing Connection object is null.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

        IDataReader? r = null;
        using IDbCommand selectCommand = connection.CreateCommand();

        IDataParameter[] parameters = CommonParameterUtility.GetParameters(selectCommand, parameterCollection);
        selectCommand.CommandType = (query.ToLower().Contains("select")) ? CommandType.Text : CommandType.StoredProcedure;
        selectCommand.CommandText = query;
        selectCommand.CommandTimeout = timeout;

        foreach (IDataParameter p in parameters)
        {
            selectCommand.Parameters.Add(p);
        }

        if (ambientTransaction != null) selectCommand.Transaction = ambientTransaction;
        r = selectCommand.ExecuteReader(CommandBehavior.Default);

        return r;
    }
}