using Songhay.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Songhay.DataAccess
{
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
        public static IDataParameter GetParameter(IDbCommand dbmsCommand, string parameterName)
        {
            if (dbmsCommand == null) throw new ArgumentNullException("dbmsCommand", "The expected Data Command is not here.");

            IDataParameter param = dbmsCommand.CreateParameter();
            if (!string.IsNullOrEmpty(parameterName)) param.ParameterName = parameterName;
            return param;
        }

        /// <summary>
        /// Gets a parameter.
        /// </summary>
        /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
        /// <param name="parameterName">The name of the Parameter.</param>
        /// <param name="parameterValue">The value of the Parameter.</param>
        /// <returns>Returns an object implementing <see cref="IDataParameter"/>.</returns>
        public static IDataParameter GetParameter(IDbCommand dbmsCommand, string parameterName, object parameterValue)
        {
            if (dbmsCommand == null) throw new ArgumentNullException("dbmsCommand", "The expected Data Command is not here.");

            IDataParameter param = dbmsCommand.CreateParameter();
            if (!string.IsNullOrEmpty(parameterName)) param.ParameterName = parameterName;
            param.Value = parameterValue;
            return param;
        }

        /// <summary>
        /// Gets a parameter.
        /// </summary>
        /// <param name="dbmsCommand">The object implementing <see cref="IDbCommand"/>.</param>
        /// <param name="parameterName">The name of the Parameter.</param>
        /// <param name="parameterValue">The value of the Parameter.</param>
        /// <param name="parameterType">The <see cref="DbType"/> of the Parameter.</param>
        /// <returns>Returns an object implementing <see cref="IDataParameter"/>.</returns>
        public static IDataParameter GetParameter(IDbCommand dbmsCommand, string parameterName, object parameterValue, DbType parameterType)
        {
            var param = GetParameter(dbmsCommand, parameterName, parameterValue);
            if (param == null) throw new NullReferenceException("The expected parameter is not here.");

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
        public static IDataParameter GetParameterFromParameterMetadata(IDbCommand dbmsCommand, DataParameterMetadata parameterMeta)
        {
            if (dbmsCommand == null) throw new ArgumentNullException("dbmsCommand", "The expected Data Command is not here.");
            if (parameterMeta == null) throw new ArgumentNullException("parameterMeta", "The expected parameter metadata is not here.");

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
        public static IDataParameter[] GetParameters(IDbCommand dbmsCommand, IEnumerable parameterCollection)
        {
            if (parameterCollection == null) throw new ArgumentNullException("parameterCollection", "The expected set of parameters is not here.");

            var @default = parameterCollection as IEnumerable<IDataParameter>;
            if (@default != null) return parameterCollection.OfType<IDataParameter>().ToArray();

            var m = parameterCollection as IEnumerable<DataParameterMetadata>;
            if (m != null) return m.Select(i => GetParameterFromParameterMetadata(dbmsCommand, i)).ToArray();

            var d = parameterCollection as Dictionary<string, object>;
            if (d != null) return d.Select(i => GetParameter(dbmsCommand, i.Key, i.Value ?? ProgramTypeUtility.SqlDatabaseNull())).ToArray();

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
        /// <typeparam name="TypeOfValue">The specified type.</typeparam>
        /// <param name="parameterValue">The boxed Parameter of the specified type.</param>
        public static object GetParameterValue<TypeOfValue>(object parameterValue)
        {
            return GetParameterValue<TypeOfValue>(parameterValue, false);
        }

        /// <summary>
        /// Returns <see cref="DBNull"/>
        /// when needed based on the specified type.
        /// </summary>
        /// <typeparam name="TypeOfValue">The specified type.</typeparam>
        /// <param name="parameterValue">The boxed Parameter of the specified type.</param>
        /// <param name="returnDbNullForZero">When true, return <see cref="DBNull"/> for numeric values.</param>
        public static object GetParameterValue<TypeOfValue>(object parameterValue, bool returnDbNullForZero)
        {
            object o = parameterValue;
            Type t = typeof(TypeOfValue);

            if (parameterValue == null)
            {
                o = ProgramTypeUtility.SqlDatabaseNull();
            }
            else if (t.Equals(typeof(DateTime)))
            {
                DateTime dt = (DateTime)parameterValue;

                //CONVENTION: return DbNull for default DateTime (Jan 1900):
                if (dt == default(DateTime)) o = ProgramTypeUtility.SqlDatabaseNull();
            }
            else if (t.IsValueType)
            {
                if ((default(TypeOfValue).Equals(parameterValue)) && returnDbNullForZero) o = ProgramTypeUtility.SqlDatabaseNull();
            }

            return o;
        }
    }
}
