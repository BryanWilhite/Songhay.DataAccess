using Songhay.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Songhay.DataAccess.Models
{
    /// <summary>
    /// Represents any DBMS with the specified
    /// invariant provider name.
    /// </summary>
    public class CommonDbms : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonDbms"/> class.
        /// </summary>
        /// <param name="invariantProviderName">Name of the invariant provider.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="ArgumentNullException">
        /// invariantProviderName;The expected provider name is not here.
        /// or
        /// connectionString;The expected connection string is not here.
        /// </exception>
        public CommonDbms(string invariantProviderName, string connectionString)
        {
            if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentNullException("invariantProviderName", "The expected provider name is not here.");
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString", "The expected connection string is not here.");

            this._invariantProviderName = invariantProviderName;
            this._factory = CommonDbmsUtility.GetProviderFactory(this._invariantProviderName);

            this._connection = CommonDbmsUtility.GetConnection(this._factory, connectionString);
            this._connection.Open();
            this.OnConnectionOpen(this._connection);
        }

        /// <summary>
        /// Gets the name of the invariant provider.
        /// </summary>
        /// <value>
        /// The name of the invariant provider.
        /// </value>
        public string InvariantProviderName { get { return this._invariantProviderName; } }

        /// <summary>
        /// Gets the provider factory.
        /// </summary>
        /// <value>
        /// The provider factory.
        /// </value>
        public DbProviderFactory ProviderFactory { get { return this._factory; } }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            CommonDbmsUtility.Close(this._connection);
        }

        /// <summary>
        /// Gets the provider not supported exception.
        /// </summary>
        /// <returns></returns>
        protected virtual Exception GetProviderNotSupportedException()
        {
            return new NotSupportedException(string.Format("Provider “{0}” is not supported.", this._invariantProviderName));
        }

        /// <summary>
        /// Gets the SQL by the specified key or <see cref="CallerMemberName"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        protected virtual string GetSql([CallerMemberName] string key = null)
        {
            var sql = this._sql[key];

            return (this._invariantProviderName == CommonDbmsConstants.OdbcProvider) ?
                sql.WithOdbcStyleParameters()
                :
                sql;
        }

        /// <summary>
        /// Called when the <see cref="DbConnection"/> is open.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected virtual void OnConnectionOpen(DbConnection connection)
        {
        }

        /// <summary>
        /// Sets the SQL.
        /// </summary>
        /// <param name="sqlSetter">The SQL setter.</param>
        /// <exception cref="ArgumentNullException">sqlSetter;The expected SQL-statement setter is not here.</exception>
        protected void SetSql(Func<Dictionary<string, string>> sqlSetter)
        {
            if (sqlSetter == null) throw new ArgumentNullException("sqlSetter", "The expected SQL-statement setter is not here.");

            this._sql = sqlSetter.Invoke();
        }

        string _invariantProviderName;
        DbConnection _connection;
        DbProviderFactory _factory;
        Dictionary<string, string> _sql;
    }
}
