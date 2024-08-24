using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class changeApprovalToPenggunaIdInPR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.AddColumn<Guid>(
                name: "PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseRequest_PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                column: "PenggunaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsPurchaseRequest_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                column: "PenggunaId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsPurchaseRequest_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrsPurchaseRequest_PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.AddColumn<string>(
                name: "Approval",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
