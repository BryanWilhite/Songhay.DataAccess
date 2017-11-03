using Newtonsoft.Json;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Configuration;
using System.IO;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ConnectionStringSettings"/>.
    /// </summary>
    public static class ConnectionStringSettingsExtensions
    {
        /// <summary>
        /// Converts the <see cref="ConnectionStringSettings"/> into a connection string.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionStringAlias">The connection string alias.</param>
        /// <param name="encryptionMetadataJsonFile">The encryption metadata json file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">settings;The expected connection-string settings are not here.</exception>
        public static string ToConnectionString(this ConnectionStringSettings settings, string encryptionMetadataJsonFile)
        {
            if (settings == null) throw new ArgumentNullException("settings", "The expected connection-string settings are not here.");

            string connectionString = null;

            var connectionStringKey = ConfigurationManager.AppSettings.GetSetting("connectionStringKey", throwConfigurationErrorsException: false);

            #region delegates:

            Func<bool> hasConnectionStringKey = () =>
            {
                if (string.IsNullOrEmpty(connectionStringKey)) return false;
                if (string.IsNullOrEmpty(settings.ConnectionString)) return false;
                return settings.ConnectionString.ToLowerInvariant().Contains(connectionStringKey.ToLowerInvariant());
            };

            #endregion

            if (!hasConnectionStringKey())
            {
                connectionString = settings.ConnectionString;
            }
            else
            {
                if (!File.Exists(encryptionMetadataJsonFile)) throw new FileNotFoundException("The expected encryption metadata JSON file configuration is not here.");
                var encryptionMetaJson = File.ReadAllText(encryptionMetadataJsonFile);
                var encryptionMeta = JsonConvert.DeserializeObject<EncryptionMetadata>(encryptionMetaJson);
                connectionString = encryptionMeta.GetConnectionStringWithDecryptedValue(settings, connectionStringKey);
            }

            return connectionString;
        }
    }
}