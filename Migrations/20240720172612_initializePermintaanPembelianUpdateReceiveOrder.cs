using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializePermintaanPembelianUpdateReceiveOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcvReceiveOrder_TrsPurchaseOrder_PurchaseOrderId",
                schema: "dbo",
                table: "RcvReceiveOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TrsPurchaseOrderDetail_RcvReceiveOrder_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_TrsPurchaseOrderDetail_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.DropColumn(
                name: "ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                newName: "PembelianId");

            migrationBuilder.RenameIndex(
                name: "IX_RcvReceiveOrder_PurchaseOrderId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                newName: "IX_RcvReceiveOrder_PembelianId");

            migrationBuilder.CreateTable(
                name: "OrdPemintaan",
                schema: "dbo",
                columns: table => new
                {
                    PermintaanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermintaanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_OrdPemintaan", x => x.PermintaanId);
                    table.ForeignKey(
                        name: "FK_OrdPemintaan_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPemintaan_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdPembelian",
                schema: "dbo",
                columns: table => new
                {
                    PembelianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PembelianNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermintaanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PermintaanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_OrdPembelian", x => x.PembelianId);
                    table.ForeignKey(
                        name: "FK_OrdPembelian_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembelian_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembelian_OrdPemintaan_PermintaanId",
                        column: x => x.PermintaanId,
                        principalSchema: "dbo",
                        principalTable: "OrdPemintaan",
                        principalColumn: "PermintaanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdPermintaanDetail",
                schema: "dbo",
                columns: table => new
                {
                    PermintaanDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermintaanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OrdPermintaanDetail", x => x.PermintaanDetailId);
                    table.ForeignKey(
                        name: "FK_OrdPermintaanDetail_OrdPemintaan_PermintaanId",
                        column: x => x.PermintaanId,
                        principalSchema: "dbo",
                        principalTable: "OrdPemintaan",
                        principalColumn: "PermintaanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdPembelianDetail",
                schema: "dbo",
                columns: table => new
                {
                    PembelianDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PembelianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamaProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodeProduk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Satuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diskon = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PermintaanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiveOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_OrdPembelianDetail", x => x.PembelianDetailId);
                    table.ForeignKey(
                        name: "FK_OrdPembelianDetail_OrdPembelian_PembelianId",
                        column: x => x.PembelianId,
                        principalSchema: "dbo",
                        principalTable: "OrdPembelian",
                        principalColumn: "PembelianId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembelianDetail_OrdPemintaan_PermintaanId",
                        column: x => x.PermintaanId,
                        principalSchema: "dbo",
                        principalTable: "OrdPemintaan",
                        principalColumn: "PermintaanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdPembelianDetail_RcvReceiveOrder_ReceiveOrderId",
                        column: x => x.ReceiveOrderId,
                        principalSchema: "dbo",
                        principalTable: "RcvReceiveOrder",
                        principalColumn: "ReceiveOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelian_PenggunaId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelian_PermintaanId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "PermintaanId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelian_UserId",
                schema: "dbo",
                table: "OrdPembelian",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelianDetail_PembelianId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PembelianId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelianDetail_PermintaanId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "PermintaanId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPembelianDetail_ReceiveOrderId",
                schema: "dbo",
                table: "OrdPembelianDetail",
                column: "ReceiveOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPemintaan_PenggunaId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPemintaan_UserId",
                schema: "dbo",
                table: "OrdPemintaan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdPermintaanDetail_PermintaanId",
                schema: "dbo",
                table: "OrdPermintaanDetail",
                column: "PermintaanId");

            migrationBuilder.AddForeignKey(
                name: "FK_RcvReceiveOrder_OrdPembelian_PembelianId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                column: "PembelianId",
                principalSchema: "dbo",
                principalTable: "OrdPembelian",
                principalColumn: "PembelianId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcvReceiveOrder_OrdPembelian_PembelianId",
                schema: "dbo",
                table: "RcvReceiveOrder");

            migrationBuilder.DropTable(
                name: "OrdPembelianDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrdPermintaanDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrdPembelian",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrdPemintaan",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "PembelianId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_RcvReceiveOrder_PembelianId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                newName: "IX_RcvReceiveOrder_PurchaseOrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseOrderDetail_ReceiveOrderId",
                schema: "dbo",
                table: "TrsPurchaseOrderDetail",
                column: "ReceiveOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RcvReceiveOrder_TrsPurchaseOrder_PurchaseOrderId",
                schema: "dbo",
                table: "RcvReceiveOrder",
                column: "PurchaseOrderId",
                principalSchema: "dbo",
                principalTable: "TrsPurchaseOrder",
                principalColumn: "PurchaseOrderId",
                onDelete: ReferentialAction.Restrict);

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
    }
}
