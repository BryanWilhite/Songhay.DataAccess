using System.Data;
using Microsoft.Extensions.Logging;
using Songhay.Extensions;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataRecord"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IDataRecordExtensions
{
    /// <summary>
    /// Returns the results of <see cref="IDataRecord.GetOrdinal"/> or <c>null</c>
    /// based on the specified key (or field name).
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static int? GetReaderOrdinal(this IDataRecord? record, string? key, ILogger? logger)
    {
        int? ordinal = null;

        if (record == null)
        {
            logger?.LogErrorForMissingData<IDataRecord>();

            return ordinal;
        }

        if (string.IsNullOrEmpty(key))
        {
            logger?.LogErrorForMissingData("No key specified.");

            return ordinal;
        }

        logger?.LogInformation("Looking for ordinal for key `{Key}`", key);

        try
        {
            ordinal = record.GetOrdinal(key);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "The reader did not match the expected key.");

            return ordinal;
        }

        logger?.LogInformation("Found ordinal, {Ordinal}, for key `{Key}`", ordinal, key);

        return ordinal;
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetBoolean"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static bool? ToBooleanOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(bool) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetBoolean(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetBoolean));

            return null;
        }
    }

    /// <summary>
    /// Converts the specified <see cref="IDataRecord"/> into a boxed value
    /// based on the specified key (or field name of the reader).
    /// </summary>
    /// <param name="record">the <see cref="IDataReader"/></param>
    /// <param name="key">the key (or field name of the <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static object? ToBoxedValue(this IDataRecord? record, string? key, ILogger? logger)
    {
        int? ordinal = record.GetReaderOrdinal(key, logger);

        if (ordinal == null) return null;

        if (record!.IsDBNull(ordinal.GetValueOrDefault()))
        {
            logger?.LogWarning("Warning: key `{Key}` is null! Returning...", key);

            return null;
        }

        object o = record.GetValue(ordinal.GetValueOrDefault());
        
        return string.IsNullOrWhiteSpace($"{o}") ? null : o;
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetByte"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static byte? ToByteOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(byte) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetByte(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetByte));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetChar"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static char? ToCharOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(char) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetChar(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetChar));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDateTime"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static DateTime? ToDateTimeOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(DateTime) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetDateTime(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetDateTime));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDecimal"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static decimal? ToDecimalOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(decimal) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetDecimal(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetDecimal));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetDouble"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static double? ToDoubleOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(double) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetDouble(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetDouble));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetFloat"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static float? ToFloatOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(float) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetFloat(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetFloat));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt16"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static short? ToInt16OrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(short) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetInt16(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetInt16));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt32"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static int? ToInt32OrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(int) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetInt32(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetInt32));

            return null;
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetInt64"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static long? ToInt64OrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(long) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetInt64(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetInt64));

            return null;
        }
    }

    /// <summary>
    /// Converts the <see cref="IDataRecord"/> to the field names of the <see cref="IDataRecord"/>.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<string> ToRowNames(this IDataReader? record)
    {
        if (record == null) throw new ArgumentNullException(nameof(record), "The expected data reader is not here.");

        for (int i = 0; i < record.FieldCount; i++)
        {
            yield return record.GetName(i);
        }
    }

    /// <summary>
    /// Tries to return the value of <see cref="IDataRecord.GetString"/> or null.
    /// </summary>
    /// <param name="record">the <see cref="IDataRecord"/></param>
    /// <param name="key">the key (or field name of the underlying <see cref="IDataRecord"/>)</param>
    /// <param name="logger">the conventional <see cref="ILogger"/></param>
    public static string? ToStringOrDefault(this IDataRecord? record, string? key, ILogger? logger)
    {
        logger?.LogInformation("Looking for {Type} with key `{Key}`...", typeof(bool), key);

        int? ordinal = GetReaderOrdinal(record, key, logger);

        if(ordinal == null) return null;

        Type fieldType = record!.GetFieldType(ordinal.Value);

        if (typeof(string) != fieldType)
        {
            logger?.LogError("The expected field type is not here! Found {Type} instead. Returning null...", fieldType);

            return null;
        }

        try
        {
            return record.GetString(ordinal.Value);
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Method} failed! Returning null...", nameof(IDataRecord.GetString));

            return null;
        }
    }
}
