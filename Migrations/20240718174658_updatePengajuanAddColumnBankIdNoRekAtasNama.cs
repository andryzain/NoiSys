using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class updatePengajuanAddColumnBankIdNoRekAtasNama : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AtasNama",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomorRekening",
                schema: "dbo",
                table: "RbsPengajuan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPengajuan_BankId",
                schema: "dbo",
                table: "RbsPengajuan",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_RbsPengajuan_MsdBank_BankId",
                schema: "dbo",
                table: "RbsPengajuan",
                column: "BankId",
                principalSchema: "dbo",
                principalTable: "MsdBank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RbsPengajuan_MsdBank_BankId",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropIndex(
                name: "IX_RbsPengajuan_BankId",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropColumn(
                name: "AtasNama",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropColumn(
                name: "BankId",
                schema: "dbo",
                table: "RbsPengajuan");

            migrationBuilder.DropColumn(
                name: "NomorRekening",
                schema: "dbo",
                table: "RbsPengajuan");
        }
    }
}
