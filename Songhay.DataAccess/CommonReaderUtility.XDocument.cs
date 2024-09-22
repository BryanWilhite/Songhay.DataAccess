using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Songhay.DataAccess;

/// <summary>
/// A few static helper members
/// for <see cref="IDataReader"/>
/// and selected <c>System.Data</c> interfaces.
/// </summary>
static public partial class CommonReaderUtility
{
    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the object implementing <see cref="IDbCommand"/>.
    /// </summary>
    /// <param name="selectCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="documentElement">Document element name of the XML set.</param>
    public static XDocument GetXDocument(IDbCommand selectCommand, string? documentElement)
    {
        if (selectCommand == null) throw new ArgumentNullException(nameof(selectCommand), "The implementing SELECT command is null.");
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");

        using IDataReader reader = selectCommand.ExecuteReader(CommandBehavior.Default);
        XDocument d = GetXDocument(documentElement, reader);

        return d;
    }

    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the object implementing <see cref="IDbCommand"/>.
    /// </summary>
    /// <param name="selectCommand">The object implementing <see cref="IDbCommand"/>.</param>
    /// <param name="documentElement">Document element name of the XML set.</param>
    /// <param name="rowElement">Row element name of the XML set.</param>
    public static XDocument GetXDocument(IDbCommand? selectCommand, string? documentElement, string? rowElement)
    {
        if (selectCommand == null) throw new ArgumentNullException(nameof(selectCommand), "The implementing SELECT command is null.");
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");
        if (string.IsNullOrEmpty(rowElement)) throw new ArgumentException("The row Element name for the XPath Document was not specified.");

        using IDataReader reader = selectCommand.ExecuteReader(CommandBehavior.Default);
        XDocument d = GetXDocument(documentElement, rowElement, reader);

        return d;
    }

    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the object implementing <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="documentElement">Document element name of the XML set.</param>
    public static XDocument GetXDocument(IDbConnection? connection, string? query, string? documentElement)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The implementing Connection object is null.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");

        using IDbCommand selectCommand = connection.CreateCommand();
        selectCommand.CommandType = CommandType.Text;
        selectCommand.CommandText = query;

        using IDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
        XDocument d = GetXDocument(documentElement, reader);

        return d;
    }

    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the object implementing <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="connection">The object implementing <see cref="IDbConnection"/>.</param>
    /// <param name="query">The SELECT SQL statement.</param>
    /// <param name="documentElement">Document element name of the XML set.</param>
    /// <param name="rowElement">Row element name of the XML set.</param>
    public static XDocument GetXDocument(IDbConnection? connection, string? query, string? documentElement, string? rowElement)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection), "The implementing Connection object is null.");
        if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");
        if (string.IsNullOrEmpty(rowElement)) throw new ArgumentException("The row Element name for the XPath Document was not specified.");

        using IDbCommand selectCommand = connection.CreateCommand();

        selectCommand.CommandType = CommandType.Text;
        selectCommand.CommandText = query;

        using IDataReader reader = selectCommand.ExecuteReader(CommandBehavior.Default);
        XDocument d = GetXDocument(documentElement, rowElement, reader);

        return d;
    }

    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the data of the type <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="documentElement">Document element name of the XML set.</param>
    /// <param name="reader">Data of the type <see cref="IDataReader"/>.</param>
    /// <remarks>This member is designed for one-row data sets.</remarks>
    public static XDocument GetXDocument(string? documentElement, IDataReader? reader)
    {
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");
        if (reader == null) throw new ArgumentNullException(nameof(reader), "The Common Data Reader is null.");
        if (reader.IsClosed) throw new ArgumentException("The implementing Data Reader is closed.", nameof(reader));

        using MemoryStream ms = new ();

        using XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);

        writer.WriteStartDocument();
        writer.WriteStartElement(documentElement);

        if (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                writer.WriteRaw(string.Format("<{0}>{1}</{0}>", reader.GetName(i), reader[i]));
            }
        }

        writer.WriteFullEndElement();
        writer.WriteEndDocument();

        writer.Flush();
        ms.Position = 0;

        XDocument xDoc = new (ms);

        return xDoc;
    }

    /// <summary>
    /// Returns a <see cref="XDocument"/>
    /// based on the data of the type <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="documentElement">Document element name of the XML set.</param>
    /// <param name="rowElement">Row element name of the XML set.</param>
    /// <param name="reader">Set implementing <see cref="IDataReader"/>.</param>
    public static XDocument GetXDocument(string? documentElement, string? rowElement, IDataReader? reader)
    {
        if (string.IsNullOrEmpty(documentElement)) throw new ArgumentException("The Document Element name for the XPath Document was not specified.");
        if (string.IsNullOrEmpty(rowElement)) throw new ArgumentException("The row Element name for the XPath Document was not specified.");
        if (reader == null) throw new ArgumentNullException(nameof(reader), "The implementing Data Reader is null.");
        if (reader.IsClosed) throw new ArgumentException("The implementing Data Reader is closed.", nameof(reader));

        using MemoryStream ms = new ();

        using XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);

        writer.WriteStartDocument();
        writer.WriteStartElement(documentElement);

        while (reader.Read())
        {
            writer.WriteStartElement(rowElement);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                writer.WriteRaw(string.Format("<{0}>{1}</{0}>", reader.GetName(i), reader[i]));
            }
            writer.WriteEndElement();
        }

        writer.WriteFullEndElement();
        writer.WriteEndDocument();

        writer.Flush();
        ms.Position = 0;

        XDocument xDoc = new (ms);

        return xDoc;
    }
}
