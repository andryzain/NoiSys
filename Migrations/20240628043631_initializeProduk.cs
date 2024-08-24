using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeProduk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsdProduk",
                schema: "dbo",
                columns: table => new
                {
                    ProdukId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamaProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrincipalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    KategoriId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JumlahStok = table.Column<int>(type: "int", nullable: false),
                    SatuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HargaBeli = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HargaJual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cogs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiskonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsdProduk", x => x.ProdukId);
                    table.ForeignKey(
                        name: "FK_MsdProduk_MsdDiskon_DiskonId",
                        column: x => x.DiskonId,
                        principalSchema: "dbo",
                        principalTable: "MsdDiskon",
                        principalColumn: "DiskonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MsdProduk_MsdKategori_KategoriId",
                        column: x => x.KategoriId,
                        principalSchema: "dbo",
                        principalTable: "MsdKategori",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MsdProduk_MsdPrincipal_PrincipalId",
                        column: x => x.PrincipalId,
                        principalSchema: "dbo",
                        principalTable: "MsdPrincipal",
                        principalColumn: "PrincipalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MsdProduk_MsdSatuan_SatuanId",
                        column: x => x.SatuanId,
                        principalSchema: "dbo",
                        principalTable: "MsdSatuan",
                        principalColumn: "SatuanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsdProduk_DiskonId",
                schema: "dbo",
                table: "MsdProduk",
                column: "DiskonId");

            migrationBuilder.CreateIndex(
                name: "IX_MsdProduk_KategoriId",
                schema: "dbo",
                table: "MsdProduk",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_MsdProduk_PrincipalId",
                schema: "dbo",
                table: "MsdProduk",
                column: "PrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_MsdProduk_SatuanId",
                schema: "dbo",
                table: "MsdProduk",
                column: "SatuanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MsdProduk",
                schema: "dbo");
        }
    }
}
