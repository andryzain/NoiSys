using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class deletePenerimaanBarang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PnmPenerimaanBarang",
                schema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PnmPenerimaanBarang",
                schema: "dbo",
                columns: table => new
                {
                    PenerimaanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdukId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodePenerimaan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodeProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StokMasuk = table.Column<int>(type: "int", nullable: false),
                    StokTersedia = table.Column<int>(type: "int", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PnmPenerimaanBarang", x => x.PenerimaanId);
                    table.ForeignKey(
                        name: "FK_PnmPenerimaanBarang_MsdProduk_ProdukId",
                        column: x => x.ProdukId,
                        principalSchema: "dbo",
                        principalTable: "MsdProduk",
                        principalColumn: "ProdukId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PnmPenerimaanBarang_ProdukId",
                schema: "dbo",
                table: "PnmPenerimaanBarang",
                column: "ProdukId");
        }
    }
}
