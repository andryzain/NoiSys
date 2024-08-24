using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeRcvReceiveOrderAndRcvReceiveOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RcvReceiveOrder",
                schema: "dbo",
                columns: table => new
                {
                    ReceiveOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiveOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcvReceiveOrder", x => x.ReceiveOrderId);
                    table.ForeignKey(
                        name: "FK_RcvReceiveOrder_AspNetUsers_ReceiveById",
                        column: x => x.ReceiveById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RcvReceiveOrder_TrsPurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "TrsPurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RcvReceiveOrderDetail",
                schema: "dbo",
                columns: table => new
                {
                    ReceivedOrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiveOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamaProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodeProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Satuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RcvReceiveOrderDetail", x => x.ReceivedOrderDetailId);
                    table.ForeignKey(
                        name: "FK_RcvReceiveOrderDetail_RcvReceiveOrder_ReceiveOrderId",
                        column: x => x.ReceiveOrderId,
                        principalSchema: "dbo",
                        principalTable: "RcvReceiveOrder",
                        principalColumn: "ReceiveOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseOrderDetail_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                column: "ReceiveOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RcvReceiveOrder_PurchaseOrderId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RcvReceiveOrder_ReceiveById",
                schema: "dbo",
                table: "RcvReceiveOrder",
                column: "ReceiveById");

            migrationBuilder.CreateIndex(
                name: "IX_RcvReceiveOrderDetail_ReceiveOrderId",
                schema: "dbo",
                table: "RcvReceiveOrderDetail",
                column: "ReceiveOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrsPurchaseOrderDetail_RcvReceiveOrder_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                column: "ReceiveOrderId",
                principalSchema: "dbo",
                principalTable: "RcvReceiveOrder",
                principalColumn: "ReceiveOrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrsPurchaseOrderDetail_RcvReceiveOrder_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.DropTable(
                name: "RcvReceiveOrderDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RcvReceiveOrder",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_TrsPurchaseOrderDetail_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.DropColumn(
                name: "ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
