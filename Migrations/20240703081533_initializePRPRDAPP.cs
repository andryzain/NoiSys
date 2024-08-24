using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializePRPRDAPP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrsPurchaseRequest",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenggunaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approval = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Termin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyTotal = table.Column<int>(type: "int", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_TrsPurchaseRequest", x => x.PurchaseRequestId);
                    table.ForeignKey(
                        name: "FK_TrsPurchaseRequest_MsdPengguna_PenggunaId",
                        column: x => x.PenggunaId,
                        principalSchema: "dbo",
                        principalTable: "MsdPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrsApprovalPurchaseRequest",
                schema: "dbo",
                columns: table => new
                {
                    ApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseRequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenggunaId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approval = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keterangan = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_TrsApprovalPurchaseRequest", x => x.ApprovalId);
                    table.ForeignKey(
                        name: "FK_TrsApprovalPurchaseRequest_TrsPurchaseRequest_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "dbo",
                        principalTable: "TrsPurchaseRequest",
                        principalColumn: "PurchaseRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrsPurchaseRequestDetail",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseRequestDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TrsPurchaseRequestDetail", x => x.PurchaseRequestDetailId);
                    table.ForeignKey(
                        name: "FK_TrsPurchaseRequestDetail_TrsPurchaseRequest_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "dbo",
                        principalTable: "TrsPurchaseRequest",
                        principalColumn: "PurchaseRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrsApprovalPurchaseRequest_PurchaseRequestId",
                schema: "dbo",
                table: "TrsApprovalPurchaseRequest",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseRequest_PenggunaId",
                schema: "dbo",
                table: "TrsPurchaseRequest",
                column: "PenggunaId");

            migrationBuilder.CreateIndex(
                name: "IX_TrsPurchaseRequestDetail_PurchaseRequestId",
                schema: "dbo",
                table: "TrsPurchaseRequestDetail",
                column: "PurchaseRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrsApprovalPurchaseRequest",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TrsPurchaseRequestDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TrsPurchaseRequest",
                schema: "dbo");
        }
    }
}
