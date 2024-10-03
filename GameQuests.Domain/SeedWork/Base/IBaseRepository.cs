using System.Linq.Expressions;

namespace GameQuests.Domain.SeedWork.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression, bool useTracking = false);

        Task<List<T>> ListAsync(bool useTracking = false);

        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression, bool useTracking = false);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
