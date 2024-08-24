using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeAndUpdatePembayaranReimbursmentAndHutangUsaha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCabangId",
                schema: "dbo",
                table: "KeuHutangUsaha");

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                schema: "dbo",
                table: "RbsPersetujuan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QtyTotal",
                schema: "dbo",
                table: "RbsPersetujuan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RbsPembayaranReimbursment",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PengajuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PengajuanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NomorRekening = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AtasNama = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_RbsPembayaranReimbursment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_RbsPembayaranReimbursment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RbsPembayaranReimbursment_MsdBank_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "MsdBank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RbsPembayaranReimbursment_RbsPengajuan_PengajuanId",
                        column: x => x.PengajuanId,
                        principalSchema: "dbo",
                        principalTable: "RbsPengajuan",
                        principalColumn: "PengajuanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RbsPembayaranReimbursment_BankId",
                schema: "dbo",
                table: "RbsPembayaranReimbursment",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPembayaranReimbursment_PengajuanId",
                schema: "dbo",
                table: "RbsPembayaranReimbursment",
                column: "PengajuanId");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPembayaranReimbursment_UserId",
                schema: "dbo",
                table: "RbsPembayaranReimbursment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RbsPembayaranReimbursment",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                schema: "dbo",
                table: "RbsPersetujuan");

            migrationBuilder.DropColumn(
                name: "QtyTotal",
                schema: "dbo",
                table: "RbsPersetujuan");

            migrationBuilder.AddColumn<Guid>(
                name: "BankCabangId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
