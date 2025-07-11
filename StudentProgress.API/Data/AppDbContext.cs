using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentProgress.API.Models.Auth;
using StudentProgress.API.Models.Students;

namespace StudentProgress.API.Data
{
    // Fix: Change IdentityDbContext<Guid> to IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Progress> ProgressRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
