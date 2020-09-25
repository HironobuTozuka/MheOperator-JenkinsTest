using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangedRouteCosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 44,
                column: "routeCost",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 98,
                column: "routeCost",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 101,
                column: "routeCost",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 102,
                column: "routeCost",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 103,
                column: "routeCost",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 104,
                column: "routeCost",
                value: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 44,
                column: "routeCost",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 98,
                column: "routeCost",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 101,
                column: "routeCost",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 102,
                column: "routeCost",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 103,
                column: "routeCost",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 104,
                column: "routeCost",
                value: 1);
        }
    }
}
