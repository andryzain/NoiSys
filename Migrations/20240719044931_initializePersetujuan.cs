using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializePersetujuan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RbsPersetujuan",
                schema: "dbo",
                columns: table => new
                {
                    PersetujuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersetujuanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PengajuanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PengajuanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NomorRekening = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AtasNama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_RbsPersetujuan", x => x.PersetujuanId);
                    table.ForeignKey(
                        name: "FK_RbsPersetujuan_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RbsPersetujuan_MsdBank_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "MsdBank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RbsPersetujuan_RbsPengajuan_PengajuanId",
                        column: x => x.PengajuanId,
                        principalSchema: "dbo",
                        principalTable: "RbsPengajuan",
                        principalColumn: "PengajuanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RbsPengajuanDetail_PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail",
                column: "PersetujuanId");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPersetujuan_BankId",
                schema: "dbo",
                table: "RbsPersetujuan",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPersetujuan_PengajuanId",
                schema: "dbo",
                table: "RbsPersetujuan",
                column: "PengajuanId");

            migrationBuilder.CreateIndex(
                name: "IX_RbsPersetujuan_UserId",
                schema: "dbo",
                table: "RbsPersetujuan",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RbsPengajuanDetail_RbsPersetujuan_PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail",
                column: "PersetujuanId",
                principalSchema: "dbo",
                principalTable: "RbsPersetujuan",
                principalColumn: "PersetujuanId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RbsPengajuanDetail_RbsPersetujuan_PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail");

            migrationBuilder.DropTable(
                name: "RbsPersetujuan",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_RbsPengajuanDetail_PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail");

            migrationBuilder.DropColumn(
                name: "PersetujuanId",
                schema: "dbo",
                table: "RbsPengajuanDetail");
        }
    }
}
