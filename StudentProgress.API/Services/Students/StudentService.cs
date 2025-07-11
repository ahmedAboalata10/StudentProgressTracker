using StudentProgress.API.IRepositories;
using StudentProgress.API.IServices.Students;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;

        public StudentService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
            => await _studentRepo.GetAllAsync();

        public async Task<Student?> GetStudentByIdAsync(Guid id)
            => await _studentRepo.GetByIdAsync(id);

        public async Task<Student?> GetStudentWithProgressAsync(Guid id)
            => await _studentRepo.GetWithProgressAsync(id);

        public async Task AddStudentAsync(Student student)
        {
            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _studentRepo.Update(student);
            await _studentRepo.SaveAsync();
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student != null)
            {
                await _studentRepo.DeleteAsync(id);
                await _studentRepo.SaveAsync();
            }
        }
    }

}
