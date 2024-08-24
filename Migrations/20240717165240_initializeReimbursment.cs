using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeReimbursment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RbsPengajuan",
                schema: "dbo",
                columns: table => new
                {
                    PengajuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PengajuanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_RbsPengajuan", x => x.PengajuanId);
                });

            migrationBuilder.CreateTable(
                name: "RbsPengajuanDetail",
                schema: "dbo",
                columns: table => new
                {
                    PengajuanDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PengajuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamaItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodeItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nominal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_RbsPengajuanDetail", x => x.PengajuanDetailId);
                    table.ForeignKey(
                        name: "FK_RbsPengajuanDetail_RbsPengajuan_PengajuanId",
                        column: x => x.PengajuanId,
                        principalSchema: "dbo",
                        principalTable: "RbsPengajuan",
                        principalColumn: "PengajuanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RbsPengajuanDetail_PengajuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail",
                column: "PengajuanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RbsPengajuanDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RbsPengajuan",
                schema: "dbo");
        }
    }
}
