using Songhay.Models;
using System.Collections;
using System.Data;

namespace Songhay.DataAccess;

/// <summary>
/// A few static helper members
/// for <see cref="System.Data.Common.DbParameter"/>
/// and selected <c>System.Data</c> interfaces.
/// </summary>
public static class CommonParameterUtility
{
    /// <summary>
    /// Gets a parameter.
    /// </summary>
    /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="parameterName">The name of the Parameter.</param>
    /// <returns>Returns an object implementing <see cref="IDataParameter"/>.</returns>
    public static IDataParameter GetParameter(IDbCommand? dbmsCommand, string? parameterName)
    {
        if (dbmsCommand == null) throw new ArgumentNullException(nameof(dbmsCommand), "The expected Data Command is not here.");

        IDataParameter param = dbmsCommand.CreateParameter();
        if (!string.IsNullOrEmpty(parameterName)) param.ParameterName = parameterName;

        return param;
    }

    /// <summary>
    /// Gets a <see cref="IDataParameter"/>.
    /// </summary>
    /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="parameterName">The name of the Parameter.</param>
    /// <param name="parameterValue">The value of the Parameter.</param>
    /// <returns>Returns an object implementing <see cref="IDataParameter"/>.</returns>
    public static IDataParameter GetParameter(IDbCommand? dbmsCommand, string parameterName, object parameterValue)
    {
        if (dbmsCommand == null) throw new ArgumentNullException(nameof(dbmsCommand), "The expected Data Command is not here.");

        IDataParameter param = dbmsCommand.CreateParameter();

        if (!string.IsNullOrEmpty(parameterName)) param.ParameterName = parameterName;
        param.Value = parameterValue;

        return param;
    }

    /// <summary>
    /// Gets a <see cref="IDataParameter"/>.
    /// </summary>
    /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="parameterName">The name of the Parameter.</param>
    /// <param name="parameterValue">The value of the Parameter.</param>
    /// <param name="parameterType">The <see cref="DbType"/> of the Parameter.</param>
    /// <returns>Returns an object implementing <see cref="IDataParameter"/>.</returns>
    public static IDataParameter GetParameter(IDbCommand? dbmsCommand, string parameterName, object parameterValue, DbType parameterType)
    {
        var param = GetParameter(dbmsCommand, parameterName, parameterValue) ??
            throw new NullReferenceException("The expected parameter is not here.");

        param.DbType = parameterType;

        return param;
    }

    /// <summary>
    /// Gets the parameter.
    /// </summary>
    /// <param name="dbmsCommand">The DBMS command.</param>
    /// <param name="parameterMeta">The parameter meta.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// dbmsCommand;The expected Data Command is not here.
    /// or
    /// parameterMeta;The expected parameter metadata is not here.
    /// </exception>
    public static IDataParameter GetParameterFromParameterMetadata(IDbCommand? dbmsCommand, DataParameterMetadata? parameterMeta)
    {
        if (dbmsCommand == null) throw new ArgumentNullException(nameof(dbmsCommand), "The expected Data Command is not here.");
        if (parameterMeta == null) throw new ArgumentNullException(nameof(parameterMeta), "The expected parameter metadata is not here.");

        IDataParameter param = dbmsCommand.CreateParameter();

        param.ParameterName = parameterMeta.ParameterName;
        param.DbType = parameterMeta.DbType;
        param.Direction = parameterMeta.ParameterDirection;
        param.SourceColumn = parameterMeta.SourceColumn;
        param.SourceVersion = parameterMeta.DataRowVersion;
        param.Value = parameterMeta.ParameterValue ?? ProgramTypeUtility.SqlDatabaseNull();

        return param;
    }

    /// <summary>
    /// Gets an array of objects implementing <see cref="IDataParameter"/>.
    /// </summary>
    /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="parameterCollection">A collection of parameters.</param>
    /// <returns>Returns an array of objects implementing <see cref="IDataParameter"/>.</returns>
    /// <remarks>
    /// Supported collections:
    ///     IEnumerable&lt;IDataParameter&gt; [pass-through]
    ///     IEnumerable&lt;DataParameterMetadata&gt;
    ///     Dictionary&lt;string, object&gt;
    /// </remarks>
    public static IDataParameter[] GetParameters(IDbCommand? dbmsCommand, IEnumerable? parameterCollection)
    {
        if (parameterCollection == null) throw new ArgumentNullException(nameof(parameterCollection), "The expected set of parameters is not here.");

        if (parameterCollection is IEnumerable<IDataParameter>) return parameterCollection.OfType<IDataParameter>().ToArray();

        if (parameterCollection is IEnumerable<DataParameterMetadata> m) return m.Select(i => GetParameterFromParameterMetadata(dbmsCommand, i)).ToArray();

        if (parameterCollection is Dictionary<string, object> d) return d.Select(i => GetParameter(dbmsCommand, i.Key, i.Value ?? ProgramTypeUtility.SqlDatabaseNull())).ToArray();

        throw new NotSupportedException(@"
The parameter collection is not supported.

Supported collections:
    IEnumerable<IDataParameter> [pass-through]
    IEnumerable<DataParameterMetadata>
    Dictionary<string, object>
");
    }

    /// <summary>
    /// Returns <see cref="System.DBNull"/>
    /// when needed based on the specified type.
    /// </summary>
    /// <typeparam name="TValue">The specified type.</typeparam>
    /// <param name="parameterValue">The boxed Parameter of the specified type.</param>
    public static object? GetParameterValue<TValue>(object? parameterValue) => GetParameterValue<TValue>(parameterValue, false);

    /// <summary>
    /// Returns <see cref="DBNull"/>
    /// when needed based on the specified type.
    /// </summary>
    /// <typeparam name="TValue">The specified type.</typeparam>
    /// <param name="parameterValue">The boxed Parameter of the specified type.</param>
    /// <param name="returnDbNullForZero">When true, return <see cref="DBNull"/> for numeric values.</param>
    public static object? GetParameterValue<TValue>(object? parameterValue, bool returnDbNullForZero)
    {
        object? o = parameterValue;
        Type t = typeof(TValue);

        if (parameterValue == null)
        {
            o = ProgramTypeUtility.SqlDatabaseNull();
        }
        else if (t.Equals(typeof(DateTime)))
        {
            DateTime dt = (DateTime)parameterValue;

            //CONVENTION: return DbNull for default DateTime (Jan 1900):
            if (dt == default) o = ProgramTypeUtility.SqlDatabaseNull();
        }
        else if (t.IsValueType)
        {
            if ((default(TValue) ?? new object()).Equals(parameterValue) && returnDbNullForZero) o = ProgramTypeUtility.SqlDatabaseNull();
        }

        return o;
    }
}