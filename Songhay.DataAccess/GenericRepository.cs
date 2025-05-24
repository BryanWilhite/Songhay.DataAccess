using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Songhay.DataAccess.Abstractions;

namespace Songhay.DataAccess;


public class GenericRepository<T, TKey>(DbContext dbContext, int allLimit = 1000) : IGenericRepository<T, TKey> where T : class where TKey : struct
{
    public async Task<T?> DeleteAsync(TKey key)
    {
        T? entity = await RetrieveByKeyAsync(key);

        if (entity == null) return null;

        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> InsertAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<IReadOnlyCollection<T>> RetrieveAllAsync(TKey key) => await dbContext.Set<T>().Take(allLimit).ToArrayAsync();

    public async Task<T?> RetrieveByKeyAsync(TKey key) => await dbContext.Set<T>().FindAsync(key);

    public async Task<T?> SearchByAsync(Expression<Func<T, bool>> predicate) => await dbContext.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<T?> UpdateAsync(T updatedEntity, TKey key)
    {
        T? currentEntity = await RetrieveByKeyAsync(key);

        if (currentEntity == null) return null;

        EntityEntry<T> entry = dbContext.Entry(currentEntity);
        entry.CurrentValues.SetValues(updatedEntity);
        entry.State = EntityState.Modified;

        await dbContext.SaveChangesAsync();

        return updatedEntity;
    }
}
