using StudentProgress.API.Models.Students;

namespace StudentProgress.API.IServices.Students
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(Guid id);
        Task<Student?> GetStudentWithProgressAsync(Guid id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(Guid id);
    }

}
