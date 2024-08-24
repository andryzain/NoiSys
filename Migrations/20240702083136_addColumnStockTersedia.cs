using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnStockTersedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JumlahStock",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                newName: "StokTersedia");

            migrationBuilder.AddColumn<int>(
                name: "StokMasuk",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StokMasuk",
                schema: "dbo",
                table: "PnmPenerimaanBarang");

            migrationBuilder.RenameColumn(
                name: "StokTersedia",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                newName: "JumlahStock");
        }
    }
}
