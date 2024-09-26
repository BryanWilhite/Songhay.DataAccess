using System.Data;
using System.Data.Common;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="DataSet"/>
/// </summary>
public static partial class DataSetExtensions
{
    /// <summary>
    /// Converts the <see cref="IEnumerable{T}"/> into a data table mappings.
    /// </summary>
    /// <param name="setTableNames">The set table names.</param>
    /// <exception cref="System.ArgumentNullException">setTableNames;The expected DataSet names are not here.</exception>
    public static IReadOnlyCollection<DataTableMapping> ToDataTableMappings(this IEnumerable<string>? setTableNames)
    {
        if (setTableNames == null) return Array.Empty<DataTableMapping>();

        return setTableNames
            .Select((name, i) => new DataTableMapping($"Table{i}", name))
            .ToArray();
    }

    /// <summary>
    /// Converts the <see cref="IEnumerable{T}"/> into a data table mappings.
    /// </summary>
    /// <param name="pairs">The pairs.</param>
    /// <exception cref="System.ArgumentNullException">
    /// pairs;The expected pairs are not here.
    /// or
    /// pairs;The expected pairs are not here.
    /// </exception>
    public static IReadOnlyCollection<DataTableMapping> ToDataTableMappings(this IEnumerable<KeyValuePair<string, string>>? pairs)
    {
        if (pairs == null) return Array.Empty<DataTableMapping>();

        return pairs
            .Select(pair => new DataTableMapping(pair.Key, pair.Value))
            .ToArray();
    }
}
