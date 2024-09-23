using System.Data;
using System.Data.Common;
using Songhay.Extensions;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="DataSet"/>
/// </summary>
public static partial class DataSetExtensions
{
    /// <summary>
    /// Loads the specified command.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="command">The command.</param>
    /// <param name="setTableNames">The set table names.</param>
    /// <param name="invariantProviderName">Name of the invariant provider.</param>
    public static DataSet Load(this DataSet? set, DbCommand? command, IReadOnlyCollection<string>? setTableNames, string? invariantProviderName)
    {
        IReadOnlyCollection<DataTableMapping> mappings = setTableNames.ToDataTableMappings();

        return set.Load(command, mappings, invariantProviderName);
    }

    /// <summary>
    /// Loads the specified command.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="command">The command.</param>
    /// <param name="pairs">The pairs.</param>
    /// <param name="invariantProviderName">Name of the invariant provider.</param>
    public static DataSet Load(this DataSet? set, DbCommand? command, IReadOnlyCollection<KeyValuePair<string, string>>? pairs, string? invariantProviderName)
    {
        IReadOnlyCollection<DataTableMapping> mappings = pairs.ToDataTableMappings();

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
    public static DataSet Load(this DataSet? set, DbCommand? command, IReadOnlyCollection<DataTableMapping>? mappings, string? invariantProviderName)
    {
        if (set == null) throw new ArgumentNullException(nameof(set), "The expected set of data is not here.");
        if (command == null) throw new ArgumentNullException(nameof(command), "The expected command is not here.");
        if (mappings == null || !mappings.Any()) throw new ArgumentNullException(nameof(mappings), "The expected table mappings are not here.");
        if (string.IsNullOrEmpty(invariantProviderName)) throw new ArgumentNullException(nameof(invariantProviderName), "The expected invariant provider name.");

        using DbDataAdapter adapter = (DbDataAdapter)CommonDbmsUtility
            .GetAdapter(invariantProviderName)
            .ToReferenceTypeValueOrThrow();

        adapter.TableMappings.AddRange(mappings.ToArray());
        adapter.SelectCommand = command;
        adapter.Fill(set);

        return set;
    }
}