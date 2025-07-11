using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProgress.API.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("rade", "Students", "Grade");
            migrationBuilder.RenameColumn("IFullName", "Students", "FullName");
            migrationBuilder.RenameColumn("GsDeleted", "Students", "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Grade", "Students", "rade");
            migrationBuilder.RenameColumn("FullName", "Students", "IFullName");
            migrationBuilder.RenameColumn("IsDeleted", "Students", "GsDeleted");
        }

    }
}
