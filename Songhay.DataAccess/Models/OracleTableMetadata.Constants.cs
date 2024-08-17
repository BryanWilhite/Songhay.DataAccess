namespace Songhay.DataAccess.Models;

/// <summary>
/// Represents output from <c>SYS.ALL_TAB_COLUMNS</c>.
/// </summary>
public partial class OracleTableMetadata
{
    /// <summary>
    /// The Oracle <c>SYS.ALL_TAB_COLUMNS</c> parameterized SQL.
    /// </summary>
    /// <remarks>
    /// Note that <c>TABLE_NAME</c> in the SQL below can specify a View name as well.
    /// </remarks>
    public const string OracleSysAllTabColumnsSql = @"
select
  COLUMN_ID
, COLUMN_NAME
, NULLABLE
, DATA_TYPE
, DATA_PRECISION
, DATA_LENGTH
, DATA_SCALE
, DATA_DEFAULT
, TABLE_NAME
from
  SYS.ALL_TAB_COLUMNS
where
  TABLE_NAME = :tableOrViewName
";
}