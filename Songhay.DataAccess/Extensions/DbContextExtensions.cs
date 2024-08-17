using Microsoft.EntityFrameworkCore;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="DbContext"/>
/// </summary>
public static partial class DbContextExtensions
{
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