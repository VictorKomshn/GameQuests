using GameQuests.Domain.SeedWork.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameQuests.Infrastructure.Data.Repositories
{
    public class RepositoryBase<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly GameQuestDataConext _context;

        public RepositoryBase(GameQuestDataConext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, bool useTracking = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!useTracking)
            {
                query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> ListAsync(bool useTracking = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!useTracking)
            {
                query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>> expression, bool useTracking = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!useTracking)
            {
                query.AsNoTracking();
            }
            return await query.Where(expression).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
