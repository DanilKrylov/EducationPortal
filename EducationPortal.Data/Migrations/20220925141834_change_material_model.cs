using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.Data.Migrations
{
    public partial class change_material_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pdf_Name",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Video_Name",
                table: "Materials");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pdf_Name",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video_Name",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
