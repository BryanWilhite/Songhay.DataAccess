using Songhay.DataAccess.Models;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="OracleTableMetadata"/>
/// </summary>
/// <remarks>
/// For research details, see “Oracle Data Types”
/// [https://docs.oracle.com/cd/B28359_01/server.111/b28318/datatype.htm]
/// </remarks>
public static class OracleTableMetadataExtensions
{
    /// <summary>
    /// Converts the <see cref="OracleTableMetadata"/> into a data annotations or <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="metadata">The metadata.</param>
    public static string ToDataAnnotationsOrEmpty(this OracleTableMetadata? metadata)
    {
        string annotations = string.Empty;
        if (metadata == null) return annotations;
        if (string.IsNullOrEmpty(metadata.DataType)) return annotations;

        string dbTypeName = metadata.DataType.ToLowerInvariant();
        bool isNullable = metadata.IsNullable.HasValue && metadata.IsNullable.GetValueOrDefault();
        string? columnName = metadata.ColumnName.ToCamelCaseFromUnderscores();
        const string newLinePlus4 = "\n    ";

        const string maxLengthTemplate = "[MaxLength({0}, ErrorMessage = \"{1} cannot exceed {0} characters.\")]";
        const string minLengthTemplate = "[MinLength({0}, ErrorMessage = \"{1} cannot have less than {0} characters.\")]";

        if (dbTypeName.Contains("varchar"))
        {
            annotations = isNullable ?
                string.Format(string.Concat(maxLengthTemplate, newLinePlus4), metadata.DataLength, columnName)
                :
                string.Format(string.Concat("[Required]", newLinePlus4, maxLengthTemplate, newLinePlus4), metadata.DataLength, columnName);
        }
        else if (dbTypeName.Contains("char"))
        {
            annotations = isNullable ?
                string.Format(string.Concat(maxLengthTemplate, newLinePlus4, minLengthTemplate, newLinePlus4), metadata.DataLength, columnName)
                :
                string.Format(string.Concat("[Required]", newLinePlus4, maxLengthTemplate, newLinePlus4, minLengthTemplate, newLinePlus4), metadata.DataLength, columnName);
        }

        return annotations;
    }

    /// <summary>
    /// Converts the <see cref="OracleTableMetadata"/> into a .NET type name.
    /// </summary>
    /// <param name="metadata">The metadata.</param>
    /// <remarks>
    /// Reference: “Oracle to .NET type mapping” [https://www.devart.com/dotconnect/oracle/docs/DataTypeMapping.html]
    /// </remarks>
    public static string? ToDotNetTypeName(this OracleTableMetadata? metadata)
    {
        if (metadata == null) return null;
        if (string.IsNullOrEmpty(metadata.DataType)) return "object";

        string typeName = "string";

        switch (metadata.DataType.ToLowerInvariant())
        {
            case "date":
                typeName = string.Concat("DateTime", metadata.IsNullable.GetValueOrDefault() ? "?" : string.Empty);
                break;

            case "number":
                if (metadata.DataScale.GetValueOrDefault().Equals(0) || (metadata.DataScale == null))
                {
                    typeName = (metadata.DataLength > 22) ? "long" : "int";
                }
                else
                {
                    typeName = (metadata.DataPrecision.GetValueOrDefault() > 16) ? "double" : "decimal";
                }
                typeName = string.Concat(typeName, metadata.IsNullable.GetValueOrDefault() ? "?" : string.Empty);
                break;
        }

        return typeName;
    }
}