using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProgress.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.UtcNow;

            // Sample student data
            var students = new[]
            {
                new { Id = Guid.NewGuid(), FullName = "Ahmed Mohamed", Grade = "A" },
                new { Id = Guid.NewGuid(), FullName = "Sara Ali", Grade = "B" },
                new { Id = Guid.NewGuid(), FullName = "Omar Hassan", Grade = "A" },
                new { Id = Guid.NewGuid(), FullName = "Mona Youssef", Grade = "C" },
                new { Id = Guid.NewGuid(), FullName = "Youssef Adel", Grade = "B" },
                new { Id = Guid.NewGuid(), FullName = "Salma Tarek", Grade = "A" },
                new { Id = Guid.NewGuid(), FullName = "Hassan Nabil", Grade = "C" },
                new { Id = Guid.NewGuid(), FullName = "Layla Saeed", Grade = "B" },
                new { Id = Guid.NewGuid(), FullName = "Mohamed Fathy", Grade = "A" },
                new { Id = Guid.NewGuid(), FullName = "Nour El-Din", Grade = "C" }
            };

            foreach (var student in students)
            {
                migrationBuilder.InsertData(
                    table: "Students",
                    columns: new[] { "Id", "FullName", "Grade", "IsDeleted", "InsertAt", "UpdateAt", "InsertBy", "UpdateBy", "TenantId" },
                    values: new object[] { student.Id, student.FullName, student.Grade, false, now, now, "seed", "seed", "GlobalExperts" }
                );

                // Insert 5 progress records for each student
                for (int i = 1; i <= 5; i++)
                {
                    migrationBuilder.InsertData(
                        table: "ProgressRecords",
                        columns: new[] { "Id", "Subject", "CompletionPercent", "PerformanceScore", "TimeSpent", "LastActivity", "StudentId", "IsDeleted", "InsertAt", "UpdateAt", "InsertBy", "UpdateBy", "TenantId" },
                        values: new object[]
                        {
                    Guid.NewGuid(),
                    $"Subject {i}",
                    70 + i * 5,
                    60 + i * 3,
                    TimeSpan.FromMinutes(40 + i * 2),
                    now.AddDays(-i),
                    student.Id,
                    false,
                    now,
                    now,
                    "seed",
                    "seed",
                    "GlobalExperts"
                        }
                    );
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optional: delete seeded data if necessary
            // Normally you'd delete by known GUIDs or filter by InsertBy = 'seed'
        }

    }
}
