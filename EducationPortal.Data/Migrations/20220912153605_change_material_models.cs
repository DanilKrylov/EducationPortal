using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.Data.Migrations
{
    public partial class change_material_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Materials",
                newName: "Pdf_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pdf_Name",
                table: "Materials",
                newName: "Author");
        }
    }
}
