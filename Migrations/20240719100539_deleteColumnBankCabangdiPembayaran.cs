using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class deleteColumnBankCabangdiPembayaran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmPembayaran_MsdBankCabang_BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran");

            migrationBuilder.DropIndex(
                name: "IX_AdmPembayaran_BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran");

            migrationBuilder.DropColumn(
                name: "BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmPembayaran_BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "BankCabangId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmPembayaran_MsdBankCabang_BankCabangId",
                schema: "dbo",
                table: "AdmPembayaran",
                column: "BankCabangId",
                principalSchema: "dbo",
                principalTable: "MsdBankCabang",
                principalColumn: "BankCabangId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
