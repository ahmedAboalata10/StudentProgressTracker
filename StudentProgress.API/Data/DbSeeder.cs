    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using StudentProgress.API.Models.Auth;
    using System;
    using System.Threading.Tasks;
namespace StudentProgress.API.Data
{

    public static class DbSeeder
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Create Roles
            var roles = new[] { "Admin", "Teacher" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            // 2. Create Admin User
            var adminEmail = "admin@school.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123"); // strong password
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3. Create Teacher User
            var teacherEmail = "teacher@school.com";
            var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
            if (teacherUser == null)
            {
                teacherUser = new ApplicationUser
                {
                    UserName = "teacher1",
                    Email = teacherEmail,
                    FullName = "Default Teacher",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(teacherUser, "Teacher@123");
                await userManager.AddToRoleAsync(teacherUser, "Teacher");
            }
        }
    }

}
