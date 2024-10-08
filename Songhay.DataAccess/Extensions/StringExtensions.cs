﻿using System.Text.RegularExpressions;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="System.String"/>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Escapes the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string Escape(this string? value) => string.IsNullOrEmpty(value) ? "NULL" : $"'{value.Trim().Replace("'", "''")}'";

    /// <summary>
    /// Converts the <see cref="string"/> into a boolean.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static bool ToBoolean(this string? data) => data.ToNullableBoolean().GetValueOrDefault();

    /// <summary>
    /// Converts the <see cref="string"/> to camelcase from underscores.
    /// </summary>
    /// <param name="name">The name.</param>
    public static string? ToCamelCaseFromUnderscores(this string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        var words = name
            .Split('_')
            .Select(i =>
            {
                if (i.Length <= 2) return i.ToUpperInvariant();

                var chars = i
                    .ToLowerInvariant()
                    .ToCharArray()
                    .Select((j, index) => (index == 0) ? char.ToUpperInvariant(j) : j)
                    .ToArray();
                var s = new string(chars);
                return s;
            })
            .ToArray();

        return string.Join(string.Empty, words);
    }

    /// <summary>
    /// Converts the boxed <see cref="string"/> into CSV cell format.
    /// </summary>
    /// <param name="data">The data.</param>
    public static string ToCsvCell(this object? data) => $"{data}".ToCsvCell();

    /// <summary>
    /// Converts the <see cref="string"/> into CSV cell format.
    /// </summary>
    /// <param name="data">The data.</param>
    public static string ToCsvCell(this string? data) => string.Concat("\"", $"{data}".Replace("\"", "\"\""), "\"");

    /// <summary>
    /// Converts the <see cref="string"/> into a nullable boolean.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static bool? ToNullableBoolean(this string? data)
    {
        if (string.IsNullOrEmpty(data)) return null;

        var s = data.ToLowerInvariant();

        return s switch
        {
            "1" or "y" or "yes" => true,
            "0" or "n" or "no" => false,
            _ => null
        };
    }

    /// <summary>
    /// Returns the specified <see cref="string"/> ODBC style parameters.
    /// </summary>
    /// <param name="sql">The SQL.</param>
    public static string? WithOdbcStyleParameters(this string? sql) => string.IsNullOrEmpty(sql) ? null : Regex.Replace(sql, @"\:\w+", "?");
}