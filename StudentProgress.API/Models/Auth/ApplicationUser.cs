using Microsoft.AspNetCore.Identity;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Models.Auth
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public ICollection<Student> AssignedStudents { get; set; }
    }

}
