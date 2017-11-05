using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="EncryptionMetadata"/>
    /// </summary>
    public static class EncryptionMetadataExtensions
    {
        /// <summary>
        /// Gets the connection string with decrypted value.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// encryptionMeta;The expected metadata is not here.
        /// or
        /// connectionString;The expected configuration settings are not here.
        /// </exception>
        /// <exception cref="System.NullReferenceException"></exception>
        public static string GetConnectionStringWithDecryptedValue(this EncryptionMetadata encryptionMeta, string connectionString, string connectionStringKey)
        {
            if (encryptionMeta == null) throw new ArgumentNullException("encryptionMeta", "The expected metadata is not here.");
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString", "The expected configuration settings are not here.");

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            var key = builder.Keys.OfType<string>().FirstOrDefault(i => i.ToLowerInvariant() == connectionStringKey);
            if (key == null) throw new NullReferenceException(string.Format("The expected key “{0}” in connection string is not here.", connectionStringKey));

            var decryptedValue = encryptionMeta.Decrypt(builder[connectionStringKey].ToString());
            builder[connectionStringKey] = decryptedValue;

            return builder.ConnectionString;
        }

        /// <summary>
        /// Gets the connection string with decrypted value.
        /// </summary>
        /// <param name="encryptionMeta">The encryption meta.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">settings;The expected configuration settings are not here.</exception>
        public static string GetConnectionStringWithDecryptedValue(this EncryptionMetadata encryptionMeta, ConnectionStringSettings settings, string connectionStringKey)
        {
            if (settings == null) throw new ArgumentNullException("settings", "The expected configuration settings are not here.");

            return encryptionMeta.GetConnectionStringWithDecryptedValue(settings.ConnectionString, connectionStringKey);
        }
    }
}
