#if NET5_0

using Songhay.DataAccess.Extensions;
using Songhay.Extensions;
using Songhay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
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
        public CommonDbms(string invariantProviderName, string connectionString) : this(invariantProviderName, connectionString, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonDbms"/> class.
        /// </summary>
        /// <param name="invariantProviderName">Name of the invariant provider.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <param name="encryptionMetaJson">The encryption meta json.</param>
        /// <exception cref="System.ArgumentNullException">
        /// invariantProviderName;The expected provider name is not here.
        /// or
        /// connectionString;The expected connection string is not here.
        /// </exception>
        public CommonDbms(string invariantProviderName, string connectionString, string connectionStringKey, string encryptionMetaJson)
        {
            if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentNullException("invariantProviderName", "The expected provider name is not here.");
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString", "The expected connection string is not here.");

            this._invariantProviderName = invariantProviderName;
            this._factory = CommonDbmsUtility.GetProviderFactory(this._invariantProviderName);

            if (!string.IsNullOrEmpty(encryptionMetaJson))
            {
                if (File.Exists(encryptionMetaJson)) encryptionMetaJson = File.ReadAllText(encryptionMetaJson);
                var encryptionMeta = JsonConvert.DeserializeObject<EncryptionMetadata>(encryptionMetaJson);
                connectionString = (!string.IsNullOrEmpty(connectionStringKey)) ?
                    encryptionMeta.GetConnectionStringWithDecryptedValue(connectionString, connectionStringKey)
                    :
                    encryptionMeta.Decrypt(connectionString);
            }

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
            return new NotSupportedException(string.Format("Provider �{0}� is not supported.", this._invariantProviderName));
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
        /// <exception cref="System.ArgumentNullException">sqlSetter;The expected SQL-statement setter is not here.</exception>
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

#endif