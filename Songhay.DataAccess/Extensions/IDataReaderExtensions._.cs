using System.Data;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataReader"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IDataReaderExtensions
{
    /// <summary>
    /// Converts the <see cref="string"/> into a date time.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this IDataReader? reader, string? key)
    {
        var d = reader.ToNullableDateTime(key);
        if (!d.HasValue) ThrowNullReferenceException(key);

        return d.GetValueOrDefault();
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a decimal.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static decimal ToDecimal(this IDataReader? reader, string? key)
    {
        var d = reader.ToNullableDecimal(key);
        if (!d.HasValue) ThrowNullReferenceException(key);

        return d.GetValueOrDefault();
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a int.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static int ToInt(this IDataReader? reader, string? key)
    {
        var i = reader.ToNullableInt(key);
        if (!i.HasValue) ThrowNullReferenceException(key);

        return i.GetValueOrDefault();
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a long.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static long ToLong(this IDataReader? reader, string? key)
    {
        var l = reader.ToNullableLong(key);
        if (!l.HasValue) ThrowNullReferenceException(key);

        return l.GetValueOrDefault();
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable boolean.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static bool? ToNullableBoolean(this IDataReader? reader, string? key)
    {
        var s = reader.ToString(key);
        if (string.IsNullOrEmpty(s)) return null;

        s = s.ToLowerInvariant();

        return s switch
        {
            "1" or "y" or "yes" => true,
            "0" or "n" or "no" => false,
            _ => Convert.ToBoolean(s)
        };
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable date time.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static DateTime? ToNullableDateTime(this IDataReader? reader, string? key)
    {
        var o = reader.ToValue(key);

        return o == DBNull.Value || o == null ? null : Convert.ToDateTime(o);
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable decimal.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static decimal? ToNullableDecimal(this IDataReader? reader, string? key)
    {
        var o = reader.ToValue(key);

        return o == DBNull.Value || o == null ? null : Convert.ToDecimal(o);
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable int.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static int? ToNullableInt(this IDataReader? reader, string? key)
    {
        var o = reader.ToValue(key);

        return o == DBNull.Value || o == null ? null : Convert.ToInt32(o);
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable long.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static long? ToNullableLong(this IDataReader? reader, string? key)
    {
        var o = reader.ToValue(key);

        return o == DBNull.Value || o == null ? null : Convert.ToInt64(o);
    }

    /// <summary>
    /// Converts the <see cref="IDataReader"/> into <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <exception cref="ArgumentNullException">The expected reader is not here.</exception>
    /// <remarks>
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
    /// Converts the <see cref="string"/> into a string.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    public static string? ToString(this IDataReader? reader, string? key)
    {
        var o = reader.ToValue(key);

        return o?.ToString();
    }

    /// <summary>
    /// Converts the <see cref="string"/> into a value.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="key">The key.</param>
    public static object? ToValue(this IDataReader? reader, string? key)
    {
        if (reader == null) return null;
        if (string.IsNullOrEmpty(key)) return null;

        int i = default;

        try
        {
            i = reader.GetOrdinal(key);
        }
        catch (IndexOutOfRangeException ex) { Throw(ex, key); }

        object o = reader.GetValue(i);

        return string.IsNullOrWhiteSpace($"{o}") ? null : reader.GetValue(i);
    }

    static void Throw(IndexOutOfRangeException ex, string? key) => throw new IndexOutOfRangeException($"The expected column name, `{key}`, is not here.", ex);

    static void ThrowNullReferenceException(string? key) => throw new NullReferenceException($"The expected data for column name, `{key}`, is not here.");
}