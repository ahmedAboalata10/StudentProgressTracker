using System.Linq.Expressions;
using StudentProgress.API.Models;

namespace StudentProgress.API.IRepositories
{

    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(Guid id);
        Task<int> CountAsync();
        Task SaveAsync();
        Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> GetQueryableAsync();

    }


}
