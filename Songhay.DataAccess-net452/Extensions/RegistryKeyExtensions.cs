using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="RegistryKey"/>.
    /// </summary>
    public static class RegistryKeyExtensions
    {
        /// <summary>
        /// Gets the data source names.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetDataSourceNames(this RegistryKey rootKey)
        {
            if (rootKey == null) return Enumerable.Empty<string>();
            var regKey = rootKey.OpenSubKey(@"Software\ODBC\ODBC.INI\ODBC Data Sources");
            if (regKey == null) return Enumerable.Empty<string>();
            return regKey.GetValueNames();
        }
    }
}
