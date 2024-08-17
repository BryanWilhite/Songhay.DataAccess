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
    public static IReadOnlyCollection<DataTableMapping> ToDataTableMappings(this IReadOnlyCollection<string>? setTableNames)
    {
        if ((setTableNames == null) || !setTableNames.Any()) throw new ArgumentNullException(nameof(setTableNames), "The expected DataSet names are not here.");

        return setTableNames
            .Select((x, i) => new DataTableMapping($"Table{i}", x))
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
    public static IReadOnlyCollection<DataTableMapping> ToDataTableMappings(this IReadOnlyCollection<KeyValuePair<string, string>>? pairs)
    {
        if (pairs == null || !pairs.Any()) throw new ArgumentNullException(nameof(pairs), "The expected pairs are not here.");

        return pairs
            .Select(i => new DataTableMapping(i.Key, i.Value))
            .ToArray();
    }
}