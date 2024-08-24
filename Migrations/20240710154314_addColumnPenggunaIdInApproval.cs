using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnPenggunaIdInApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrsApprovalPurchaseRequest_PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "PenggunaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "PenggunaId",
                principalSchema: "dbo",
                principalTable: "MsdPengguna",
                principalColumn: "PenggunaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_MsdPengguna_PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrsApprovalPurchaseRequest_PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");
        }
    }
}
