using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updateColumnPersetujuanDeletePersetujuanNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersetujuanNumber",
                schema: "dbo",
                table: "RbsPersetujuan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersetujuanNumber",
                schema: "dbo",
                table: "RbsPersetujuan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
