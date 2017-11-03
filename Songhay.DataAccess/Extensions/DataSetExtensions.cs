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
    public static class DataSetExtensions
    {
        /// <summary>
        /// Loads the specified command.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="command">The command.</param>
        /// <param name="setTableNames">The set table names.</param>
        /// <param name="invariantProviderName">Name of the invariant provider.</param>
        public static DataSet Load(this DataSet set, DbCommand command, IEnumerable<string> setTableNames, string invariantProviderName)
        {
            var mappings = setTableNames.ToDataTableMappings();
            return set.Load(command, mappings, invariantProviderName);
        }

        /// <summary>
        /// Loads the specified command.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="command">The command.</param>
        /// <param name="pairs">The pairs.</param>
        /// <param name="invariantProviderName">Name of the invariant provider.</param>
        public static DataSet Load(this DataSet set, DbCommand command, IEnumerable<KeyValuePair<string, string>> pairs, string invariantProviderName)
        {
            var mappings = pairs.ToDataTableMappings();
            return set.Load(command, mappings, invariantProviderName);
        }

        /// <summary>
        /// Loads the specified command.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="command">The command.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="invariantProviderName">Name of the invariant provider.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// set;The expected set of data is not here.
        /// or
        /// command;The expected command is not here.
        /// or
        /// mappings;The expected table mappings are not here.
        /// or
        /// mappings;The expected table mappings are not here.
        /// </exception>
        public static DataSet Load(this DataSet set, DbCommand command, IEnumerable<DataTableMapping> mappings, string invariantProviderName)
        {
            if (set == null) throw new ArgumentNullException("set", "The expected set of data is not here.");
            if (command == null) throw new ArgumentNullException("command", "The expected command is not here.");
            if ((mappings == null) || (!mappings.Any())) throw new ArgumentNullException("mappings", "The expected table mappings are not here.");
            if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentNullException("invariantProviderName", "The expected invariant provider name.");

            using (var adapter = CommonDbmsUtility.GetAdapter(invariantProviderName))
            {
                adapter.TableMappings.AddRange(mappings.ToArray());
                adapter.SelectCommand = command;
                adapter.Fill(set);
            }

            return set;
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{System.String}"/> into a data table mappings.
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
        /// Converts the <see cref="IEnumerable{KeyValuePair{System.String, System.String}}"/> into a data table mappings.
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
