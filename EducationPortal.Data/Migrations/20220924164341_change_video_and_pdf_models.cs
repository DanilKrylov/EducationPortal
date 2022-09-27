using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.Data.Migrations
{
    public partial class change_video_and_pdf_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Video_Data",
                table: "Materials");

            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Video_DataId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaterilaDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    ByteData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterilaDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterilaDatas_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_DataId",
                table: "Materials",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Video_DataId",
                table: "Materials",
                column: "Video_DataId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterilaDatas_MaterialId",
                table: "MaterilaDatas",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterilaDatas_DataId",
                table: "Materials",
                column: "DataId",
                principalTable: "MaterilaDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterilaDatas_Video_DataId",
                table: "Materials",
                column: "Video_DataId",
                principalTable: "MaterilaDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterilaDatas_DataId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterilaDatas_Video_DataId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "MaterilaDatas");

            migrationBuilder.DropIndex(
                name: "IX_Materials_DataId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_Video_DataId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Video_DataId",
                table: "Materials");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Materials",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Video_Data",
                table: "Materials",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
