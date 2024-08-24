using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updateColumnPengajuan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_RbsPengajuan_UserId",
                schema: "dbo",
                table: "RbsPengajuan",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RbsPengajuan_AspNetUsers_UserId",
                schema: "dbo",
                table: "RbsPengajuan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RbsPengajuan_AspNetUsers_UserId",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropIndex(
                name: "IX_RbsPengajuan_UserId",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
