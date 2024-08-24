using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updateColumnPermintaanAddStatusDisetujuiOleh2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrdPemintaan_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "DisetujuiOlehId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "DisetujuiOlehId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdPemintaan_MsdPengguna_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan");

            migrationBuilder.DropIndex(
                name: "IX_OrdPemintaan_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPemintaan");
        }
    }
}
