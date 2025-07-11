
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.IRepositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetWithProgressAsync(Guid studentId);
        Task<List<Student>> GetAllWithProgressAsync();

    }

}
