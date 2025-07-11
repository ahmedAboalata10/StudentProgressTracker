using Microsoft.EntityFrameworkCore;
using StudentProgress.API.Data;
using StudentProgress.API.IRepositories;
using System.Linq.Expressions;
using StudentProgress.API.Models;
namespace StudentProgress.API.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(e => !e.IsDeleted).Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.InsertAt = DateTime.UtcNow;
            entity.UpdateAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdateAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null) return;
            entity.IsDeleted = true;
            entity.UpdateAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync(e => !e.IsDeleted);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(x => !x.IsDeleted);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IQueryable<T>> GetQueryableAsync()
        {
            return await Task.FromResult(_dbSet.AsQueryable().Where(x => !x.IsDeleted));
        }

    }


}
