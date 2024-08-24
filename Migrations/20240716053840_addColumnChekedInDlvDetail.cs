using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnChekedInDlvDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DlvPurchaseOrderDetail_DlvDeliveryOrder_DeliveryOrderId",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DlvPurchaseOrderDetail",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail");

            migrationBuilder.RenameTable(
                name: "DlvPurchaseOrderDetail",
                schema: "dbo",
                newName: "DlvDeliveryOrderDetail",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_DlvPurchaseOrderDetail_DeliveryOrderId",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail",
                newName: "IX_DlvDeliveryOrderDetail_DeliveryOrderId");

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DlvDeliveryOrderDetail",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail",
                column: "DeliveryOrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DlvDeliveryOrderDetail_DlvDeliveryOrder_DeliveryOrderId",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail",
                column: "DeliveryOrderId",
                principalSchema: "dbo",
                principalTable: "DlvDeliveryOrder",
                principalColumn: "DeliveryOrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DlvDeliveryOrderDetail_DlvDeliveryOrder_DeliveryOrderId",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DlvDeliveryOrderDetail",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "Checked",
                schema: "dbo",
                table: "DlvDeliveryOrderDetail");

            migrationBuilder.RenameTable(
                name: "DlvDeliveryOrderDetail",
                schema: "dbo",
                newName: "DlvPurchaseOrderDetail",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_DlvDeliveryOrderDetail_DeliveryOrderId",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail",
                newName: "IX_DlvPurchaseOrderDetail_DeliveryOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DlvPurchaseOrderDetail",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail",
                column: "DeliveryOrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DlvPurchaseOrderDetail_DlvDeliveryOrder_DeliveryOrderId",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail",
                column: "DeliveryOrderId",
                principalSchema: "dbo",
                principalTable: "DlvDeliveryOrder",
                principalColumn: "DeliveryOrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
