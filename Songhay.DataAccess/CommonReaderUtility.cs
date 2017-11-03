using System;
using System.Collections;
using System.Data;

namespace Songhay.DataAccess
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.Data.Common.DbDataReader"/>
    /// and selected <c>System.Data</c> interfaces.
    /// </summary>
    public static partial class CommonReaderUtility
    {
        /// <summary>
        /// Returns an instance of <see cref="System.Data.IDataReader"/>
        /// based on the instance of <see cref="System.Data.Common.DbConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="System.Data.Common.DbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        public static IDataReader GetReader(IDbConnection cnn, string query)
        {
            return GetReader(cnn, query, null);
        }

        /// <summary>
        /// Returns an instance of <see cref="System.Data.IDataReader"/>
        /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        /// <param name="parameterCollection">A list of parameters.</param>
        public static IDataReader GetReader(IDbConnection cnn, string query, IEnumerable parameterCollection)
        {
            return GetReader(cnn, query, parameterCollection, 30);
        }

        /// <summary>
        /// Returns an instance of <see cref="System.Data.IDataReader"/>
        /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        /// <param name="parameterCollection">A list of parameters.</param>
        /// <param name="timeout">Command timeout in seconds.</param>
        public static IDataReader GetReader(IDbConnection cnn, string query, IEnumerable parameterCollection, int timeout)
        {
            return GetReader(cnn, query, parameterCollection, timeout, null);
        }

        /// <summary>
        /// Returns an instance of <see cref="System.Data.IDataReader"/>
        /// based on the object implementing <see cref="System.Data.IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        /// <param name="parameterCollection">A list of parameters.</param>
        /// <param name="timeout">Command timeout in seconds.</param>
        /// <param name="ambientTransaction">The ambient <see cref="System.Data.IDbTransaction"/> implementation.</param>
        public static IDataReader GetReader(IDbConnection cnn, string query, IEnumerable parameterCollection, int timeout, IDbTransaction ambientTransaction)
        {
            if (cnn == null) throw new ArgumentNullException("connection", "The implementing Connection object is null.");
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

            IDataReader r = null;
            using (IDbCommand selectCommand = cnn.CreateCommand())
            {
                var parameters = CommonParameterUtility.GetParameters(selectCommand, parameterCollection);
                selectCommand.CommandType = (query.ToLower().Contains("select")) ? CommandType.Text : CommandType.StoredProcedure;
                selectCommand.CommandText = query;
                selectCommand.CommandTimeout = timeout;

                if (parameters != null)
                {
                    foreach (IDataParameter p in parameters)
                    {
                        selectCommand.Parameters.Add(p);
                    }
                }
                if (ambientTransaction != null) selectCommand.Transaction = ambientTransaction;
                r = selectCommand.ExecuteReader(CommandBehavior.Default);
            }
            return r;
        }
    }
}
