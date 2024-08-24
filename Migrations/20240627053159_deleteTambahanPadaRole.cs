﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiSys.Migrations
{
    public partial class deleteTambahanPadaRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Keterangan",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Menu",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Keterangan",
                table: "AspNetRoles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Menu",
                table: "AspNetRoles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "AspNetRoles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
