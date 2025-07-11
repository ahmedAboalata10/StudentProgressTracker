using StudentProgress.API.IRepositories;
using StudentProgress.API.IServices.Students;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IStudentRepository studentRepo,
            ILogger<StudentService> logger)
        {
            _studentRepo = studentRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all students.");
                return await _studentRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all students.");
                throw;
            }
        }

        public async Task<Student?> GetStudentByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching student with ID: {StudentId}", id);
                return await _studentRepo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching student by ID: {StudentId}", id);
                throw;
            }
        }

        public async Task<Student?> GetStudentWithProgressAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching student with progress. ID: {StudentId}", id);
                return await _studentRepo.GetWithProgressAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching student with progress. ID: {StudentId}", id);
                throw;
            }
        }

        public async Task AddStudentAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Adding new student: {FullName}", student.FullName);
                await _studentRepo.AddAsync(student);
                await _studentRepo.SaveAsync();
                _logger.LogInformation("Student added successfully: {FullName}", student.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding student: {FullName}", student.FullName);
                throw;
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Updating student: {StudentId}", student.Id);
                _studentRepo.Update(student);
                await _studentRepo.SaveAsync();
                _logger.LogInformation("Student updated successfully: {StudentId}", student.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating student: {StudentId}", student.Id);
                throw;
            }
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting student with ID: {StudentId}", id);
                var student = await _studentRepo.GetByIdAsync(id);
                if (student != null)
                {
                    await _studentRepo.DeleteAsync(id);
                    await _studentRepo.SaveAsync();
                    _logger.LogInformation("Student deleted successfully: {StudentId}", id);
                }
                else
                {
                    _logger.LogWarning("Delete failed. Student not found: {StudentId}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting student: {StudentId}", id);
                throw;
            }
        }
    }
}
