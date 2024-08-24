using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updateNamePermintaanDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelian_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelianDetail_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPemintaan_AspNetUsers_UserId",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPermintaanDetail_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPermintaanDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdPemintaan",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.RenameTable(
                name: "OrdPemintaan",
                schema: "dbo",
                newName: "OrdPermintaan",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPemintaan_UserId",
                schema: "dbo",
                table: "OrdPermintaan",
                newName: "IX_OrdPermintaan_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPemintaan_PenggunaId",
                schema: "dbo",
                table: "OrdPermintaan",
                newName: "IX_OrdPermintaan_PenggunaId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPemintaan_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPermintaan",
                newName: "IX_OrdPermintaan_DisetujuiOlehId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdPermintaan",
                schema: "dbo",
                table: "OrdPermintaan",
                column: "PermintaanId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelian_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPermintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelianDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPermintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPermintaan_AspNetUsers_UserId",
                schema: "dbo",
                table: "OrdPermintaan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPermintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPermintaan",
                column: "DisetujuiOlehId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPermintaan_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "OrdPermintaan",
                column: "PenggunaId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPermintaanDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPermintaanDetail",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPermintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelian_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelian");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPembelianDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPermintaan_AspNetUsers_UserId",
                schema: "dbo",
                table: "OrdPermintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPermintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPermintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPermintaan_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "OrdPermintaan");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdPermintaanDetail_OrdPermintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPermintaanDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdPermintaan",
                schema: "dbo",
                table: "OrdPermintaan");

            migrationBuilder.RenameTable(
                name: "OrdPermintaan",
                schema: "dbo",
                newName: "OrdPemintaan",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPermintaan_UserId",
                schema: "dbo",
                table: "OrdPemintaan",
                newName: "IX_OrdPemintaan_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPermintaan_PenggunaId",
                schema: "dbo",
                table: "OrdPemintaan",
                newName: "IX_OrdPemintaan_PenggunaId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdPermintaan_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan",
                newName: "IX_OrdPemintaan_DisetujuiOlehId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdPemintaan",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "PermintaanId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelian_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPemintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPembelianDetail_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPemintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPemintaan_AspNetUsers_UserId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "DisetujuiOlehId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "PenggunaId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPermintaanDetail_OrdPemintaan_PermintaanId",
                schema: "dbo",
                table: "OrdPermintaanDetail",
                column: "PermintaanId",
                principalSchema: "dbo",
                principalTable: "OrdPemintaan",
                principalColumn: "PermintaanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
