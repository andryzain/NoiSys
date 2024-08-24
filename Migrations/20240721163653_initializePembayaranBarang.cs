using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializePembayaranBarang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdPembayaranBarang",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PembelianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PembelianNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisetujuiOlehId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_OrdPembayaranBarang", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_OrdPembayaranBarang_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembayaranBarang_MsdBank_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "MsdBank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembayaranBarang_MsdPengguna_DisetujuiOlehId",
                        column: x => x.DisetujuiOlehId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembayaranBarang_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembayaranBarang_OrdPembelian_PembelianId",
                        column: x => x.PembelianId,
                        principalSchema: "dbo",
                        principalTable: "OrdPembelian",
                        principalColumn: "PembelianId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembayaranBarang_BankId",
                schema: "dbo",
                table: "OrdPembayaranBarang",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembayaranBarang_DisetujuiOlehId",
                schema: "dbo",
                table: "OrdPembayaranBarang",
                column: "DisetujuiOlehId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembayaranBarang_PembelianId",
                schema: "dbo",
                table: "OrdPembayaranBarang",
                column: "PembelianId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembayaranBarang_PenggunaId",
                schema: "dbo",
                table: "OrdPembayaranBarang",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembayaranBarang_UserId",
                schema: "dbo",
                table: "OrdPembayaranBarang",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdPembayaranBarang",
                schema: "dbo");
        }
    }
}
