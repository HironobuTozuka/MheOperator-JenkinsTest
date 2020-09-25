using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class adddedToteStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "mhe",
                table: "Totes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "mhe",
                table: "Totes");
        }
    }
}
