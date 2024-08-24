﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class initializeKategori : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsdKategori",
                schema: "dbo",
                columns: table => new
                {
                    KategoriId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeKategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamaKategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_MsdKategori", x => x.KategoriId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MsdKategori",
                schema: "dbo");
        }
    }
}
