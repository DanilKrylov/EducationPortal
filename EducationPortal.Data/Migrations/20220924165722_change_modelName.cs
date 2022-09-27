using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.Data.Migrations
{
    public partial class change_modelName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterilaDatas_Materials_MaterialId",
                table: "MaterilaDatas");

            migrationBuilder.DropIndex(
                name: "IX_MaterilaDatas_MaterialId",
                table: "MaterilaDatas");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "MaterilaDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "MaterilaDatas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterilaDatas_MaterialId",
                table: "MaterilaDatas",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterilaDatas_Materials_MaterialId",
                table: "MaterilaDatas",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
