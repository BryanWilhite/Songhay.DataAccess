using System.Linq;
using System.Text.RegularExpressions;

namespace Songhay.DataAccess.Extensions
{
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
        public static string Escape(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "NULL";
            else return string.Format("'{0}'", value.Trim().Replace("'", "''"));
        }

        /// <summary>
        /// Converts the <see cref="string"/> into a boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static bool ToBoolean(this string data)
        {
            return data.ToNullableBoolean().GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="string"/> to camelcase from underscores.
        /// </summary>
        /// <param name="name">The name.</param>
        public static string ToCamelCaseFromUnderscores(this string name)
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
        /// Converts the <see cref="string"/> into a nullable boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static bool? ToNullableBoolean(this string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var s = data.ToLowerInvariant();

            if (s.Equals("1")) return true;
            if (s.Equals("y")) return true;
            if (s.Equals("yes")) return true;

            if (s.Equals("0")) return false;
            if (s.Equals("n")) return false;
            if (s.Equals("no")) return false;

            return null;
        }

        /// <summary>
        /// Withes the ODBC style parameters.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public static string WithOdbcStyleParameters(this string sql)
        {
            if (string.IsNullOrEmpty(sql)) return null;
            return Regex.Replace(sql, @"\:\w+", "?");
        }
    }
}
