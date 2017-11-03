using System;
using System.Collections.Generic;
using System.Data;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IDataReader"/>
    /// </summary>
    public static partial class IDataReaderExtensions
    {
        /// <summary>
        /// Converts the <see cref="String"/> into a date time.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this IDataReader reader, string key)
        {
            var d = reader.ToNullableDateTime(key);
            if (!d.HasValue) ThrowNullReferenceException(key);
            return d.GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this IDataReader reader, string key)
        {
            var d = reader.ToNullableDecimal(key);
            if (!d.HasValue) ThrowNullReferenceException(key);
            return d.GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a int.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int ToInt(this IDataReader reader, string key)
        {
            var i = reader.ToNullableInt(key);
            if (!i.HasValue) ThrowNullReferenceException(key);
            return i.GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a long.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static long ToLong(this IDataReader reader, string key)
        {
            var l = reader.ToNullableLong(key);
            if (!l.HasValue) ThrowNullReferenceException(key);
            return l.GetValueOrDefault();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable boolean.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool? ToNullableBoolean(this IDataReader reader, string key)
        {
            var s = reader.ToString(key);
            if (string.IsNullOrEmpty(s)) return null;

            s = s.ToLowerInvariant();

            if (s.Equals("1") || s.Equals("y") || s.Equals("yes")) return true;
            if (s.Equals("0") || s.Equals("n") || s.Equals("no")) return false;

            return Convert.ToBoolean(s);
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable date time.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTime(this IDataReader reader, string key)
        {
            var o = reader.ToValue(key);
            if (o == DBNull.Value) return null;
            if (o.ToString().Equals(string.Empty)) return null;
            return Convert.ToDateTime(o);
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable decimal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static decimal? ToNullableDecimal(this IDataReader reader, string key)
        {
            var o = reader.ToValue(key);
            if (o == DBNull.Value) return null;
            if (o.ToString().Equals(string.Empty)) return null;
            return Convert.ToDecimal(o);
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable int.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int? ToNullableInt(this IDataReader reader, string key)
        {
            var o = reader.ToValue(key);
            if (o == DBNull.Value) return null;
            return Convert.ToInt32(o);
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a nullable long.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static long? ToNullableLong(this IDataReader reader, string key)
        {
            var o = reader.ToValue(key);
            if (o == DBNull.Value) return null;
            return Convert.ToInt64(o);
        }

        /// <summary>
        /// Converts the <see cref="IDataReader"/> into <see cref="IEnumerable{object[]}"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.ArgumentNullException">The expected reader is not here.</exception>
        /// <remarks>
        /// for more info, see “Consuming a DataReader with LINQ”
        /// [http://www.thinqlinq.com/default/consuming-a-datareader-with-linq]
        /// </remarks>
        public static IEnumerable<object[]> ToRowValues(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException("The expected data reader is not here.");

            while (reader.Read())
            {
                var row = new object[reader.FieldCount];
                reader.GetValues(row);
                yield return row;
            }
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string ToString(this IDataReader reader, string key)
        {
            var o = reader.ToValue(key);
            if (o == null) return null;

            return o.ToString();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object ToValue(this IDataReader reader, string key)
        {
            if (reader == null) return null;

            int i = default(int);
            try
            {
                i = reader.GetOrdinal(key);
            }
            catch (IndexOutOfRangeException ex) { Throw(ex, key); }

            return reader.GetValue(i);
        }

        static void Throw(IndexOutOfRangeException ex, string key)
        {
            throw new IndexOutOfRangeException(string.Format("The expected column name, “{0},” is not here.", key), ex);
        }

        static void ThrowNullReferenceException(string key)
        {
            throw new NullReferenceException(string.Format("The expected data for column name, “{0},” is not here.", key));
        }
    }
}
