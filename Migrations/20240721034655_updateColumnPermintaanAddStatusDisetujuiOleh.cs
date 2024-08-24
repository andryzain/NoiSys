using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updateColumnPermintaanAddStatusDisetujuiOleh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "OrdPemintaan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "OrdPemintaan");
        }
    }
}
