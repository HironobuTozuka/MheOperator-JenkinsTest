using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddedRoutesFromRackAToCA_P : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Routes",
                columns: new[] { "id", "deviceId", "isDefaultRoute", "locationId", "locationTypeId", "routeCost", "routedLocationId", "routedLocationTypeId" },
                values: new object[,]
                {
                    { 107, "CA_P2", true, null, 2, 1, 2, null },
                    { 106, "CA_P1", true, null, 2, 1, 1, null },
                    { 108, "CA_P2", true, null, 3, 1, 2, null },
                    { 105, "CA_P1", true, null, 1, 1, 1, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 108);
        }
    }
}
