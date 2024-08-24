using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnBengkelInPR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseRequest_BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                column: "BengkelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsPurchaseRequest_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                column: "BengkelId",
                principalSchema: "dbo",
                principalTable: "MsdBengkel",
                principalColumn: "BengkelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsPurchaseRequest_MsdBengkel_BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrsPurchaseRequest_BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "BengkelId",
                schema: "dbo",
                table: "TrsPurchaseRequest");
        }
    }
}
