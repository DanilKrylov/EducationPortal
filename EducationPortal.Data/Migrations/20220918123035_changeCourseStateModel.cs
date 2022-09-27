using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.Data.Migrations
{
    public partial class changeCourseStateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "CourseStates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "CourseStates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
