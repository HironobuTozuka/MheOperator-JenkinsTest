using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddedRoutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Routes",
                columns: new[] { "id", "deviceId", "isDefaultRoute", "locationId", "locationTypeId", "routeCost", "routedLocationId", "routedLocationTypeId" },
                values: new object[,]
                {
                    { 99, "CB_P1", true, null, 5, 1, 3, null },
                    { 96, "CB_P1", true, 23, null, 1, 3, null },
                    { 95, "CB_P1", true, 22, null, 1, 3, null },
                    { 94, "CB_P1", true, 17, null, 1, 3, null },
                    { 93, "CB_P1", true, 15, null, 1, 3, null },
                    { 92, "CB_P1", true, 8, null, 1, 3, null },
                    { 97, "CA_P2", false, null, 3, 1, null, 3 },
                    { 100, "CA_P2", false, null, 2, 1, null, 2 },
                    { 101, "CA_P2", false, null, 2, 1, null, 3 },
                    { 102, "CA_P2", false, null, 3, 1, null, 2 },
                    { 103, "CA_P1", false, null, 1, 1, null, 2 },
                    { 104, "CA_P1", false, null, 2, 1, null, 1 },
                    { 91, "CA_P2", false, 21, null, 1, 18, null },
                    { 90, "CA_P2", true, 21, null, 1, 16, null },
                    { 98, "CA_P1", true, null, 1, 1, null, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 99);
            
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 100);

        }
    }
}
