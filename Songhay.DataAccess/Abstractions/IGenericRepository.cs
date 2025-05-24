using System.Linq.Expressions;

namespace Songhay.DataAccess.Abstractions;

public interface IGenericRepository<T, TKey> where T : class where TKey : struct
{
    Task<T?> DeleteAsync(TKey key);

    Task<T> InsertAsync(T entity);

    Task<IReadOnlyCollection<T>> RetrieveAllAsync(TKey key);

    Task<T?> RetrieveByKeyAsync(TKey key);

    Task<T?> SearchByAsync(Expression<Func<T, bool>> predicate);

    Task<T?> UpdateAsync(T updatedEntity, TKey key);
}
