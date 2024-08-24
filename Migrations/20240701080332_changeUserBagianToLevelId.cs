using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class changeUserBagianToLevelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bagian",
                schema: "dbo",
                table: "MsdPengguna");

            migrationBuilder.DropColumn(
                name: "Bagian",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                schema: "dbo",
                table: "MsdPengguna",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsdPengguna_LevelId",
                schema: "dbo",
                table: "MsdPengguna",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LevelId",
                table: "AspNetUsers",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MsdLevelPengguna_LevelId",
                table: "AspNetUsers",
                column: "LevelId",
                principalSchema: "dbo",
                principalTable: "MsdLevelPengguna",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MsdPengguna_MsdLevelPengguna_LevelId",
                schema: "dbo",
                table: "MsdPengguna",
                column: "LevelId",
                principalSchema: "dbo",
                principalTable: "MsdLevelPengguna",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MsdLevelPengguna_LevelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MsdPengguna_MsdLevelPengguna_LevelId",
                schema: "dbo",
                table: "MsdPengguna");

            migrationBuilder.DropIndex(
                name: "IX_MsdPengguna_LevelId",
                schema: "dbo",
                table: "MsdPengguna");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LevelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LevelId",
                schema: "dbo",
                table: "MsdPengguna");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Bagian",
                schema: "dbo",
                table: "MsdPengguna",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bagian",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
