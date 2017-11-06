using Songhay.DataAccess.Models;
using System.Collections.Generic;

namespace Songhay.DataAccess.TextTemplating
{
    public partial class OracleEntityGenerator
    {
        public OracleEntityGenerator(IEnumerable<OracleTableMetadata> metadata)
        {
            this.TableMetadata = metadata;
        }

        public IEnumerable<OracleTableMetadata> TableMetadata { get; private set; }
    }
}
