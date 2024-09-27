using System.Data;
using System.Xml.Linq;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="IDataReader"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IDataReaderExtensions
{
    /// <summary>
    /// Adds the <see cref="XElement"/> from <see cref="IDataReader"/> fields.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="rowElement">The row element.</param>
    /// <remarks>
    /// This is the row-level method, adding field-elements per row.
    /// </remarks>
    public static void AddXElements(this IDataReader? reader, XElement? rowElement)
    {
        if (reader == null) return;
        if (rowElement == null) return;

        for (int i = 0; i < reader.FieldCount; i++)
        {
            rowElement.Add(new XElement(reader.GetName(i), reader[i].ToString()));
        }
    }

    /// <summary>
    /// Adds the x elements.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="rootElement">The root element.</param>
    /// <param name="rowElementName">Name of the row element.</param>
    /// <remarks>
    /// This is the root-level method, adding row-elements to the root element.
    /// </remarks>
    public static void AddXElements(this IDataReader? reader, XElement? rootElement, string? rowElementName)
    {
        if (reader == null) return;
        if (rootElement == null) return;
        if (string.IsNullOrEmpty(rowElementName)) rowElementName = "row";

        while (reader.Read())
        {
            var rowElement = new XElement(rowElementName);
            reader.AddXElements(rowElement);
            rootElement.Add(rowElement);
        }
    }

    /// <summary>
    /// Converts the <see cref="IDataReader"/> into a <see cref="XDocument"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    public static XDocument ToXDocument(this IDataReader? reader) => reader.ToXDocument(rootElementName: null, rowElementName: null);

    /// <summary>
    /// Converts the <see cref="IDataReader"/> into a <see cref="XDocument"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="rootElementName">Name of the root element.</param>
    public static XDocument ToXDocument(this IDataReader? reader, string? rootElementName) => reader.ToXDocument(rootElementName, rowElementName: null);

    /// <summary>
    /// Converts the <see cref="IDataReader"/> into a <see cref="XDocument"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="rootElementName">Name of the root element.</param>
    /// <param name="rowElementName">Name of the row element.</param>
    public static XDocument ToXDocument(this IDataReader? reader, string? rootElementName, string? rowElementName)
    {
        if (reader == null) return new XDocument();

        if (string.IsNullOrEmpty(rootElementName)) rootElementName = "root";
        if (string.IsNullOrEmpty(rowElementName)) rowElementName = "row";

        var xd = XDocument.Parse($"<{rootElementName} />");
        reader.AddXElements(xd.Root, rowElementName);

        return xd;
    }
}