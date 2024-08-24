using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class changeNamaBengkelToBengkelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NamaBengkel",
                schema: "dbo",
                table: "MsdMekanik");

            migrationBuilder.AddColumn<Guid>(
                name: "BengkelId",
                schema: "dbo",
                table: "MsdMekanik",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsdMekanik_BengkelId",
                schema: "dbo",
                table: "MsdMekanik",
                column: "BengkelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MsdMekanik_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "MsdMekanik",
                column: "BengkelId",
                principalSchema: "dbo",
                principalTable: "MsdBengkel",
                principalColumn: "BengkelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MsdMekanik_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "MsdMekanik");

            migrationBuilder.DropIndex(
                name: "IX_MsdMekanik_BengkelId",
                schema: "dbo",
                table: "MsdMekanik");

            migrationBuilder.DropColumn(
                name: "BengkelId",
                schema: "dbo",
                table: "MsdMekanik");

            migrationBuilder.AddColumn<string>(
                name: "NamaBengkel",
                schema: "dbo",
                table: "MsdMekanik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
