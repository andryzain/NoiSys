using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeBankAndBankCabagAndPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsdBank",
                schema: "dbo",
                columns: table => new
                {
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamaBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_MsdBank", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "MsdBankCabang",
                schema: "dbo",
                columns: table => new
                {
                    BankCabangId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeBankCabang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamaCabang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomorRekening = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AtasNama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_MsdBankCabang", x => x.BankCabangId);
                    table.ForeignKey(
                        name: "FK_MsdBankCabang_MsdBank_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "MsdBank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdmPembayaran",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankCabangId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BengkelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_AdmPembayaran", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_AdmInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "AdmInvoice",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_MsdBank_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "MsdBank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_MsdBankCabang_BankCabangId",
                        column: x => x.BankCabangId,
                        principalSchema: "dbo",
                        principalTable: "MsdBankCabang",
                        principalColumn: "BankCabangId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_MsdBengkel_BengkelId",
                        column: x => x.BengkelId,
                        principalSchema: "dbo",
                        principalTable: "MsdBengkel",
                        principalColumn: "BengkelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmPembayaran_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "BankCabangId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_BankId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_BengkelId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "BengkelId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_InvoiceId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_PenggunaId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_UserId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MsdBankCabang_BankId",
                schema: "dbo",
                table: "MsdBankCabang",
                column: "BankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmPembayaran",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MsdBankCabang",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MsdBank",
                schema: "dbo");
        }
    }
}
