using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class changeApprovalColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.RenameColumn(
                name: "PenggunaId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CheckedBy",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                newName: "ApproveBy");

            migrationBuilder.AlterColumn<string>(
                name: "Keterangan",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDate",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                newName: "PenggunaId");

            migrationBuilder.RenameColumn(
                name: "ApproveBy",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                newName: "CheckedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Keterangan",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Approval",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
