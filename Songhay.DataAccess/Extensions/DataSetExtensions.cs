using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="DataSet"/>
    /// </summary>
    public static partial class DataSetExtensions
    {
        /// <summary>
        /// Converts the <see cref="IEnumerable{string}"/> into a data table mappings.
        /// </summary>
        /// <param name="setTableNames">The set table names.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">setTableNames;The expected DataSet names are not here.</exception>
        public static IEnumerable<DataTableMapping> ToDataTableMappings(this IEnumerable<string> setTableNames)
        {
            if ((setTableNames == null) || (!setTableNames.Any())) throw new ArgumentNullException("setTableNames", "The expected DataSet names are not here.");
            return setTableNames.Select((x, i) => new DataTableMapping(string.Format("Table{0}", i), x));
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{KeyValuePair{string, string}}"/> into a data table mappings.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// pairs;The expected pairs are not here.
        /// or
        /// pairs;The expected pairs are not here.
        /// </exception>
        public static IEnumerable<DataTableMapping> ToDataTableMappings(this IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if ((pairs == null) || (!pairs.Any())) throw new ArgumentNullException("pairs", "The expected pairs are not here.");
            return pairs.Select(i => new DataTableMapping(i.Key, i.Value));
        }
    }
}
