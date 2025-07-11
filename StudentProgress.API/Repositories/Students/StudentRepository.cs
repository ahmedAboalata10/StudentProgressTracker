using Microsoft.EntityFrameworkCore;
using StudentProgress.API.Data;
using StudentProgress.API.IRepositories;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Repositories.Students
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }

        public async Task<Student?> GetWithProgressAsync(Guid studentId)
        {
            return await _dbSet
                .Include(s => s.ProgressRecords)
                .FirstOrDefaultAsync(s => s.Id == studentId && !s.IsDeleted);
        }
        public async Task<List<Student>> GetAllWithProgressAsync()
        {
            return await _dbSet
                .Include(s => s.ProgressRecords)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

    }

}
