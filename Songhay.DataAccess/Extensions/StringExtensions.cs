using System.Text.RegularExpressions;

namespace Songhay.DataAccess.Extensions
{    /// <summary>
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
        /// Converts the <see cref="String"/> into a boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static bool ToBoolean(this string data)
        {
            return data.ToNullableBoolean().GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable boolean.
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
