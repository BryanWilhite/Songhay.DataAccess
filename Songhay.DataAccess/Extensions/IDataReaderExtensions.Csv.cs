using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Songhay.Extensions;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataReader"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IDataReaderExtensions
{
    /// <summary>
    /// Streams the contents of the <see cref="IDataReader"/>
    /// to the specified path.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="path">the path the CSV file</param>
    /// <param name="includeHeader">when <c>true</c> include CSV headers</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static void StreamToCsvFile(this IDataReader? reader, string? path, bool includeHeader, ILogger? logger)
    {
        if (reader == null)
        {
            logger?.LogErrorForMissingData<IDataReader>();

            return;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            logger?.LogError("Error: the expected path is not here.");

            return;
        }

        using FileStream stream = File.OpenWrite(path);

        if (includeHeader)
        {
            logger?.LogError("Getting headers...");

            byte[] headersData = Encoding.UTF8.GetBytes(string.Concat(reader.ToRowNames().Select(n => n.ToCsvCell()).Aggregate((a, name) => $"{a},{name}"), Environment.NewLine));
            stream.Write(headersData, 0, headersData.Length);
        }

        logger?.LogInformation("Writing to CSV file, `{Path}`...", path);

        while (reader.Read())
        {
            object[] row = new object[reader.FieldCount];
            reader.GetValues(row);

            byte[] data = Encoding.UTF8.GetBytes(string.Concat(row.Select(n => n.ToCsvCell()).Aggregate((a, datum) => $"{a},{datum}"), Environment.NewLine));
            stream.Write(data, 0, data.Length);
        }
    }
}
