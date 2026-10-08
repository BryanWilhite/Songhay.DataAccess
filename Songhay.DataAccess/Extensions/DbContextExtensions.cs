using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="DbContext"/>
/// </summary>
public static class DbContextExtensions
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
        string? sql = GetDeleteByKeySql<TEntityType>(context, keyColumn);

        if (string.IsNullOrWhiteSpace(sql)) return 0;

        return context?.Database.ExecuteSqlRaw(sql, new { key }) ?? 0;
    }

    /// <summary>
    /// Deletes an EF entity by the specified column and key, asynchronously.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="context">the <see cref="DbContext"/></param>
    /// <param name="keyColumn">The key column.</param>
    /// <param name="key">The key.</param>
    /// <returns>The number of rows affected.</returns>
    public static async Task<int> DeleteByKeyAsync<TEntityType, TKey>(this DbContext? context, string? keyColumn, TKey? key)
    {
        if (context == null) return 0;

        string? sql = GetDeleteByKeySql<TEntityType>(context, keyColumn);

        if (string.IsNullOrWhiteSpace(sql)) return 0;

        return await context.Database.ExecuteSqlRawAsync(sql, new { key });
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

    internal static string? GetDeleteByKeySql<TEntityType>(DbContext? context, string? keyColumn)
    {
        if (context == null) return null;
        if (string.IsNullOrEmpty(keyColumn) || keyColumn.Contains(';')) return null;

        IEntityType? entityType = context.Model.FindEntityType(typeof(TEntityType));
        if (entityType == null) return null;

        string? tableName = entityType.GetTableName();
        if (string.IsNullOrEmpty(tableName)) return null;

        string sql = $"DELETE FROM {tableName} WHERE {keyColumn} = @key";

        return sql;
    }
}