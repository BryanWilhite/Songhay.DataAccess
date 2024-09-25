using System.Data;
using Microsoft.Extensions.Logging;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataReader"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IDataReaderExtensions
{
    /// <summary>
    /// Returns the results of <see cref="IDataRecord.GetOrdinal"/>
    /// based on the specified key (or field name of the reader).
    /// </summary>
    /// <param name="reader">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static int? GetReaderOrdinal(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).GetReaderOrdinal(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetBoolean"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static bool? ToBooleanOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToBooleanOrDefault(key, logger);

    /// <summary>
    /// Converts the specified <see cref="IDataReader"/> into a boxed value
    /// based on the specified key (or field name of the reader).
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static object? ToBoxedValue(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToBoxedValue(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetByte"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static byte? ToByteOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToByteOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetChar"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static char? ToCharOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToCharOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDateTime"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static DateTime? ToDateTimeOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToDateTimeOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDecimal"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static decimal? ToDecimalOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToDecimalOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDouble"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static double? ToDoubleOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToDoubleOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetFloat"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static float? ToFloatOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToFloatOrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt16"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static short? ToInt16OrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToInt16OrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt32"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static int? ToInt32OrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToInt32OrDefault(key, logger);

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt64"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static long? ToInt64OrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToInt64OrDefault(key, logger);

    /// <summary>
    /// Converts the <see cref="IDataReader"/> into <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <exception cref="ArgumentNullException">The expected reader is not here.</exception>
    /// <remarks>
    /// This member uses <see cref="IDataRecord.GetValues"/> under the hood;
    /// for more info, see “Consuming a DataReader with LINQ”
    /// [http://www.thinqlinq.com/default/consuming-a-datareader-with-linq]
    /// </remarks>
    public static IEnumerable<object[]> ToRowValues(this IDataReader? reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader), "The expected data reader is not here.");

        while (reader.Read())
        {
            object[] row = new object[reader.FieldCount];
            reader.GetValues(row);

            yield return row;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetString"/> or null.
    /// </summary>
    /// <param name="reader">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static string? ToStringOrDefault(this IDataReader? reader, string? key, ILogger? logger) => (reader as IDataRecord).ToStringOrDefault(key, logger);
}
