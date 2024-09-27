using System.Collections;
using System.Data;

namespace Songhay.DataAccess;

/// <summary>
/// A few static helper members
/// for <see cref="System.Data.Common.DbDataReader"/>
/// and selected <c>System.Data</c> interfaces.
/// </summary>
static public class CommonScalarUtility
{
    /// <summary>
    /// Return a <see cref="System.Object"/>
    /// based on the DBMS query.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    public static object? GetObject(IDbConnection? connection, string? query) => GetObject(connection, query, null);

    /// <summary>
    /// Return a <see cref="System.Object"/>
    /// based on the DBMS query.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameters">The parameters.</param>
    public static object? GetObject(IDbConnection? connection, string? query, params (string Name, object? Value)[]? parameters)
    {
        IEnumerable<(string Name, object? Value)>? parameterCollection = parameters;

        return GetObject(connection, query, parameterCollection);
    }

    /// <summary>
    /// Return a <see cref="System.Object"/>
    /// based on the DBMS query.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="parameterCollection">The parameters.</param>
    public static object? GetObject(IDbConnection? connection, string? query, IEnumerable? parameterCollection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The Common Connection object is null.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

        object? o;

        using IDbCommand cmd = connection.CreateCommand();

        if (parameterCollection != null)
        {
            IReadOnlyCollection<IDataParameter> paramArray = CommonParameterUtility.GetParameters(cmd, parameterCollection);

            foreach (IDataParameter p in paramArray)
            {
                cmd.Parameters.Add(p);
            }
        }

        cmd.CommandType = CommandType.Text;
        cmd.CommandText = query;

        o = cmd.ExecuteScalar();

        return o;
    }
}
