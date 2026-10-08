using System.Runtime.CompilerServices;
using Songhay.DataAccess.Extensions;

namespace Songhay.DataAccess.Models;

/// <summary>
/// Collects and centralizes conventional SQL statements
/// for <see cref="CommonDbms"/> and other scenarios
/// </summary>
/// <param name="invariantProviderName"></param>
public class SqlStatementCollection(string invariantProviderName) : Dictionary<string, string?>
{
    /// <summary>
    /// Gets the SQL by the specified key or <see cref="CallerMemberNameAttribute"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    public string? GetSql([CallerMemberName] string? key = null)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;

        invariantProviderName.ThrowWhenNullOrWhiteSpace();

        string? sql = this[key];

        return invariantProviderName == CommonDbmsConstants.OdbcProvider ?
            sql.WithOdbcStyleParameters()
            :
            sql;
    }

    /// <summary>
    /// Sets the SQL.
    /// </summary>
    /// <param name="sqlSetter">The SQL setter.</param>
    /// <exception cref="System.ArgumentNullException">sqlSetter;The expected SQL-statement setter is not here.</exception>
    public void SetSql(Func<Dictionary<string, string?>>? sqlSetter)
    {
        ArgumentNullException.ThrowIfNull(sqlSetter);

        this.WithPairs(sqlSetter.Invoke());
    }
}
