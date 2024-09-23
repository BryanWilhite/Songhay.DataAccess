using System.Data;
using System.Text.Json.Nodes;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataReader"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IDataReaderExtensions
{
    /// <summary>
    /// Converts <see cref="IDataReader"/> to <see cref="JsonObject"/>
    /// </summary>
    /// <param name="reader">The reader.</param>
    public static JsonObject? ToJsonObject(this IDataReader? reader) => reader.ToJsonObject(rootPropertyName: null);

    /// <summary>
    /// Converts <see cref="IDataReader"/> to <see cref="JsonObject"/>
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="rootPropertyName">Name of the root property (defaults to <c>root</c>).</param>
    /// <returns></returns>
    public static JsonObject? ToJsonObject(this IDataReader? reader, string? rootPropertyName)
    {
        if (reader == null) return null;
        if (string.IsNullOrEmpty(rootPropertyName)) rootPropertyName = "root";

        JsonObject jORoot = new ();

        JsonArray jA = new ();

        while (reader.Read())
        {
            var jO = new JsonObject();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                JsonNode? node = reader.GetValue(i).ToJsonNode();

                jO.Add(reader.GetName(i), node);
            }

            jA.Add(jO);
        }

        jORoot.Add(rootPropertyName, jA);

        return jORoot;
    }
}
