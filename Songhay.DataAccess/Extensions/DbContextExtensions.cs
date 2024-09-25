using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="DbContext"/>
/// </summary>
public static partial class DbContextExtensions
{
    /// <summary>
    /// Deletes an EF entity by the specified column and key.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="context">the <see cref="DbContext"/></param>
    /// <param name="keyColumn">The key column.</param>
    /// <param name="key">The key.</param>
    /// <returns>The number of rows affected.</returns>
    public static int DeleteByKey<TEntityType, TKey>(this DbContext? context, string? keyColumn, TKey? key)
    {
        if (context == null) return default;
        if (string.IsNullOrEmpty(keyColumn)) return default;

        IEntityType? entityType = context.Model.FindEntityType(typeof(TEntityType));
        if (entityType == null) return default;

        string? tableName = entityType.GetTableName();
        if (string.IsNullOrEmpty(tableName)) return default;

        string sql = $"DELETE FROM {tableName} WHERE {keyColumn} = @key";

        return context.Database.ExecuteSqlRaw(sql, new { key });
    }

    /// <summary>
    /// Detaches the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="context">the <see cref="DbContext"/></param>
    /// <param name="entity">The entity.</param>
    public static void Detach<TEntity>(this DbContext? context, TEntity? entity) where TEntity : class
    {
        if (context == null) return;
        if(entity == null) return;

        context.Entry(entity).State = EntityState.Detached;
    }
}