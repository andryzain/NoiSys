using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeDeliveryOrderAndDeliveryOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DlvDeliveryOrder",
                schema: "dbo",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BengkelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyTotal = table.Column<int>(type: "int", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DlvDeliveryOrder", x => x.DeliveryOrderId);
                    table.ForeignKey(
                        name: "FK_DlvDeliveryOrder_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DlvDeliveryOrder_MsdBengkel_BengkelId",
                        column: x => x.BengkelId,
                        principalSchema: "dbo",
                        principalTable: "MsdBengkel",
                        principalColumn: "BengkelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DlvDeliveryOrder_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DlvDeliveryOrder_TrsPurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "TrsPurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DlvPurchaseOrderDetail",
                schema: "dbo",
                columns: table => new
                {
                    DeliveryOrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NamaProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodeProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Satuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diskon = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_DlvPurchaseOrderDetail", x => x.DeliveryOrderDetailId);
                    table.ForeignKey(
                        name: "FK_DlvPurchaseOrderDetail_DlvDeliveryOrder_DeliveryOrderId",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "dbo",
                        principalTable: "DlvDeliveryOrder",
                        principalColumn: "DeliveryOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DlvDeliveryOrder_BengkelId",
                schema: "dbo",
                table: "DlvDeliveryOrder",
                column: "BengkelId");

            migrationBuilder.CreateIndex(
                name: "IX_DlvDeliveryOrder_PenggunaId",
                schema: "dbo",
                table: "DlvDeliveryOrder",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_DlvDeliveryOrder_PurchaseOrderId",
                schema: "dbo",
                table: "DlvDeliveryOrder",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DlvDeliveryOrder_UserId",
                schema: "dbo",
                table: "DlvDeliveryOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DlvPurchaseOrderDetail_DeliveryOrderId",
                schema: "dbo",
                table: "DlvPurchaseOrderDetail",
                column: "DeliveryOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DlvPurchaseOrderDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DlvDeliveryOrder",
                schema: "dbo");
        }
    }
}
