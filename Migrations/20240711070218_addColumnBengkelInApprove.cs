using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnBengkelInApprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrsApprovalPurchaseRequest_BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "BengkelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "BengkelId",
                principalSchema: "dbo",
                principalTable: "MsdBengkel",
                principalColumn: "BengkelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrsApprovalPurchaseRequest_BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "BengkelId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");
        }
    }
}
