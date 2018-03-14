using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projectvega.Migrations
{
    public partial class SeedFearture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT Features (Name) VALUEs ('Feature1')");
            migrationBuilder.Sql("INSERT Features (Name) VALUEs ('Feature2')");
            migrationBuilder.Sql("INSERT Features (Name) VALUEs ('Feature3')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Features WHERE Name IN ('Feature1','Feature2','Feature3')");
        }
    }
}
