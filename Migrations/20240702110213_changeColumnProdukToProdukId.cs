using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class changeColumnProdukToProdukId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NamaProduk",
                schema: "dbo",
                table: "PnmPenerimaanBarang");

            migrationBuilder.AddColumn<Guid>(
                name: "ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PnmPenerimaanBarang_ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                column: "ProdukId");

            migrationBuilder.AddForeignKey(
                name: "FK_PnmPenerimaanBarang_MsdProduk_ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                column: "ProdukId",
                principalSchema: "dbo",
                principalTable: "MsdProduk",
                principalColumn: "ProdukId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PnmPenerimaanBarang_MsdProduk_ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang");

            migrationBuilder.DropIndex(
                name: "IX_PnmPenerimaanBarang_ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang");

            migrationBuilder.DropColumn(
                name: "ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang");

            migrationBuilder.AddColumn<string>(
                name: "NamaProduk",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
