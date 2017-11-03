using Newtonsoft.Json.Linq;
using System.Data;

namespace Songhay.DataAccess.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IDataReader"/>
    /// </summary>
    public static partial class IDataReaderExtensions
    {
        /// <summary>
        /// Converts <see cref="IDataReader"/> to <see cref="JObject"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="rootPropertyName">Name of the root property.</param>
        /// <returns></returns>
        public static JObject ToJObject(this IDataReader reader, string rootPropertyName)
        {
            if (reader == null) return null;
            if (string.IsNullOrEmpty(rootPropertyName)) rootPropertyName = "root";

            var jO_root = new JObject();

            var jA = new JArray();
            while (reader.Read())
            {
                var jO = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    jO.Add(reader.GetName(i), JToken.FromObject(reader[i]));
                }

                jA.Add(jO);
            }

            jO_root.Add(rootPropertyName, jA);

            return jO_root;
        }
    }
}
