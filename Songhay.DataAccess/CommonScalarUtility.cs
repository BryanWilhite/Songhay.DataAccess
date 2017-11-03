using Songhay.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Songhay.DataAccess
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.Data.Common.DbDataReader"/>
    /// and selected <c>System.Data</c> interfaces.
    /// </summary>
    static public class CommonScalarUtility
    {
        /// <summary>
        /// Return a <see cref="System.Object"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        public static object GetObject(IDbConnection connection, string query)
        {
            return GetObject(connection, query, null);
        }

        /// <summary>
        /// Return a <see cref="System.Object"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SELECT SQL statement.</param>
        /// <param name="parameterCollection">The parameters.</param>
        public static object GetObject(IDbConnection connection, string query, IEnumerable parameterCollection)
        {
            if (connection == null) throw new ArgumentNullException("connection", "The Common Connection object is null.");
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

            object o = null;

            using (IDbCommand cmd = connection.CreateCommand())
            {
                if (parameterCollection != null)
                {
                    IDataParameter[] paramArray = CommonParameterUtility.GetParameters(cmd, parameterCollection);
                    foreach (IDataParameter p in paramArray)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                o = cmd.ExecuteScalar();
            }

            return o;
        }

        /// <summary>
        /// Return a <see cref="System.String"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SQL statement.</param>
        /// <remarks>
        /// Use this member for queries like this:
        /// 
        ///     DECLARE @xml XML
        ///     SET @xml = (SELECT * FROM MyTable FOR XML AUTO, ELEMENTS, ROOT('RootElement'))
        ///     SELECT @xml
        /// 
        /// </remarks>
        public static string GetString(IDbConnection connection, string query)
        {
            return GetString(connection, query, null, true);
        }

        /// <summary>
        /// Return a <see cref="System.String"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SQL statement.</param>
        /// <param name="returnXmlForEmptySet">When <c>true</c> <see cref="Songhay.Xml.XmlUtility.GetInternalMessage(string,string[])"/> is used for empty sets.</param>
        public static string GetString(IDbConnection connection, string query, bool returnXmlForEmptySet)
        {
            return GetString(connection, query, null, returnXmlForEmptySet);
        }

        /// <summary>
        /// Return a <see cref="System.String"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SQL statement.</param>
        /// <param name="parameterCollection">The parameters.</param>
        public static string GetString(IDbConnection connection, string query, IEnumerable parameterCollection)
        {
            return GetString(connection, query, parameterCollection, true);
        }

        /// <summary>
        /// Return a <see cref="System.String"/>
        /// based on the DBMS query.
        /// </summary>
        /// <param name="connection">The object implementing <see cref="System.Data.IDbConnection"/>.</param>
        /// <param name="query">The SQL statement.</param>
        /// <param name="parameterCollection">The parameters.</param>
        /// <param name="returnXmlForEmptySet">When <c>true</c> <see cref="Songhay.Xml.XmlUtility.GetInternalMessage(string,string[])"/> is used for empty sets.</param>
        public static string GetString(IDbConnection connection, string query, IEnumerable parameterCollection, bool returnXmlForEmptySet)
        {
            if (connection == null) throw new ArgumentNullException("connection", "The Common Connection object is null.");
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("The DBMS query was not specified.");

            string s = null;

            if (connection.State != ConnectionState.Open) throw new DataException(string.Format("The Connection to the DBMS is not open."));

            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                if (parameterCollection != null)
                {
                    IDataParameter[] parameters = CommonParameterUtility.GetParameters(cmd, parameterCollection);
                    foreach (IDataParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                object o = cmd.ExecuteScalar();
                if (o is string) s = o.ToString();

                if (string.IsNullOrEmpty(s) && returnXmlForEmptySet)
                {
                    string[] sa = new string[] { "The query executed against the DBMS connection returned an empty set." };
                    s = XmlUtility.GetInternalMessage("The query Returned No Data", sa);
                }
            }
            return s;
        }
    }
}
