using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone1.Server.Migrations
{
    public partial class TeacherCourse_Visible_Deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TeacherCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "TeacherCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TeacherCourses");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "TeacherCourses");
        }
    }
}
