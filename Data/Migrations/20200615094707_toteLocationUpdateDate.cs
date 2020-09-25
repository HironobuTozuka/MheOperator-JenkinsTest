using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class toteLocationUpdateDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastLocationUpdate",
                schema: "mhe",
                table: "Totes",
                nullable: false,
                defaultValueSql: "NOW()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastLocationUpdate",
                schema: "mhe",
                table: "Totes");
        }
    }
}
