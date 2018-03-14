using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projectvega.Migrations
{
    public partial class AddphotosV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Photos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Photos",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
