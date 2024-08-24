using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnPenggunaIdInApproval2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TrsApprovalPurchaseRequest_UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_AspNetUsers_UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsApprovalPurchaseRequest_AspNetUsers_UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrsApprovalPurchaseRequest_UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
