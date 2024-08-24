using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnInPembelian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelianDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrdPembelianDetail_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail");

            migrationBuilder.DropColumn(
                name: "PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "OrdPembelian",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelian_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "DisetujuiOlehId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelian_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "DisetujuiOlehId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelian_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.DropIndex(
                name: "IX_OrdPembelian_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.DropColumn(
                name: "DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.AddColumn<Guid>(
                name: "PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelianDetail_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PermintaanId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelianDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPermintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
