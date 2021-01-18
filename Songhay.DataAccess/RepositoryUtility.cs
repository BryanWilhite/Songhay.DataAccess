using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;

namespace Songhay.DataAccess.Repository
{
    /// <summary>
    /// Static helpers for <see cref="System.Data.EntityClient"/>.
    /// </summary>
    public static partial class RepositoryUtility
    {
        /// <summary>
        /// Gets the Repository connection string.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        /// <param name="providerConnectionString">The provider connection string.</param>
        /// <param name="provider">The provider.</param>
        public static string GetRepositoryConnectionString(string metaData, string providerConnectionString, string provider = "System.Data.SqlClient")
        {
            var builder = new EntityConnectionStringBuilder();
            builder.Metadata = metaData;
            builder.Provider = provider;
            builder.ProviderConnectionString = providerConnectionString;
            return builder.ToString();
        }

        /// <summary>
        /// Gets the state of the entity.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="entity">The entity.</param>
        /// <remarks>
        /// For more detail, see “RIA Services and Entity Framework POCOs”
        /// [http://thedatafarm.com/blog/data-access/ria-services-and-entity-framework-pocos/]
        /// by Julie Lerman
        /// </remarks>
        public static EntityState GetEntityState(ObjectStateManager manager, object entity)
        {
            if(manager == null) return default(EntityState);

            ObjectStateEntry entry;
            if(manager.TryGetObjectStateEntry(entity, out entry))
                return entry.State;
            else
                return EntityState.Detached;
        }
    }
}
