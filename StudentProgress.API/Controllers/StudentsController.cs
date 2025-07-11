using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProgress.API.IServices.Students;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetProgress(Guid id)
        {
            var student = await _studentService.GetStudentWithProgressAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            await _studentService.AddStudentAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Student student)
        {
            if (id != student.Id) return BadRequest();
            await _studentService.UpdateStudentAsync(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
        [Authorize]
        [HttpGet("/api/users/{id}/students")]
        public async Task<IActionResult> GetStudentsForTeacher(Guid id)
        {
            var students = await _studentService.GetAllStudentsAsync();
            var assigned = students.Where(s => s.TeacherId == id).ToList();
            return Ok(assigned);
        }
    }
}
