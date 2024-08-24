using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class addColumnQtyDiOrderAndQtyDiterimaRcvOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Qty",
                schema: "dbo",
                table: "RcvReceiveOrderDetail",
                newName: "QtyDiTerima");

            migrationBuilder.AddColumn<int>(
                name: "QtyDiOrder",
                schema: "dbo",
                table: "RcvReceiveOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyDiOrder",
                schema: "dbo",
                table: "RcvReceiveOrderDetail");

            migrationBuilder.RenameColumn(
                name: "QtyDiTerima",
                schema: "dbo",
                table: "RcvReceiveOrderDetail",
                newName: "Qty");
        }
    }
}
