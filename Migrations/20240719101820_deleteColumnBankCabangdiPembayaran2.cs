using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class deleteColumnBankCabangdiPembayaran2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCabangId",
                schema: "dbo",
                table: "KeuPiutangUsaha");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_KeuPiutangUsaha_BankId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_KeuPiutangUsaha_UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KeuHutangUsaha_BankId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_KeuHutangUsaha_UserId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeuHutangUsaha_AspNetUsers_UserId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KeuHutangUsaha_MsdBank_BankId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                column: "BankId",
                principalSchema: "dbo",
                principalTable: "MsdBank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KeuPiutangUsaha_AspNetUsers_UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KeuPiutangUsaha_MsdBank_BankId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                column: "BankId",
                principalSchema: "dbo",
                principalTable: "MsdBank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeuHutangUsaha_AspNetUsers_UserId",
                schema: "dbo",
                table: "KeuHutangUsaha");

            migrationBuilder.DropForeignKey(
                name: "FK_KeuHutangUsaha_MsdBank_BankId",
                schema: "dbo",
                table: "KeuHutangUsaha");

            migrationBuilder.DropForeignKey(
                name: "FK_KeuPiutangUsaha_AspNetUsers_UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha");

            migrationBuilder.DropForeignKey(
                name: "FK_KeuPiutangUsaha_MsdBank_BankId",
                schema: "dbo",
                table: "KeuPiutangUsaha");

            migrationBuilder.DropIndex(
                name: "IX_KeuPiutangUsaha_BankId",
                schema: "dbo",
                table: "KeuPiutangUsaha");

            migrationBuilder.DropIndex(
                name: "IX_KeuPiutangUsaha_UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha");

            migrationBuilder.DropIndex(
                name: "IX_KeuHutangUsaha_BankId",
                schema: "dbo",
                table: "KeuHutangUsaha");

            migrationBuilder.DropIndex(
                name: "IX_KeuHutangUsaha_UserId",
                schema: "dbo",
                table: "KeuHutangUsaha");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "BankCabangId",
                schema: "dbo",
                table: "KeuPiutangUsaha",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "KeuHutangUsaha",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
