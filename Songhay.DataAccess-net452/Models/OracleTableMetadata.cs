using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Songhay.DataAccess.Models
{
    /// <summary>
    /// Represents output from <c>SYS.ALL_TAB_COLUMNS</c>.
    /// </summary>
    public partial class OracleTableMetadata
    {
        /// <summary>
        /// Gets or sets the column identifier.
        /// </summary>
        /// <value>
        /// The column identifier.
        /// </value>
        public int? ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool? IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the data precision.
        /// </summary>
        /// <value>
        /// The data precision.
        /// </value>
        public int? DataPrecision { get; set; }

        /// <summary>
        /// Gets or sets the length of the data.
        /// </summary>
        /// <value>
        /// The length of the data.
        /// </value>
        [Required]
        public int DataLength { get; set; }

        /// <summary>
        /// Gets or sets the data scale.
        /// </summary>
        /// <value>
        /// The data scale.
        /// </value>
        public int? DataScale { get; set; }

        /// <summary>
        /// Gets or sets the data default.
        /// </summary>
        /// <value>
        /// The data default.
        /// </value>
        public string DataDefault { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        [Required]
        public string TableName { get; set; }

        /// <summary>
        /// The string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(this.TableName)) sb.AppendFormat("TableName: {0}\n", this.TableName);
            if (this.ColumnId != null) sb.AppendFormat("ColumnId: {0}\n", this.ColumnId);
            if (!string.IsNullOrEmpty(this.ColumnName)) sb.AppendFormat("ColumnName: {0}\n", this.ColumnName);
            if (this.IsNullable != null) sb.AppendFormat("IsNullable: {0}\n", this.IsNullable);

            return (sb.Length > 0) ? sb.ToString() : base.ToString();
        }
    }
}
