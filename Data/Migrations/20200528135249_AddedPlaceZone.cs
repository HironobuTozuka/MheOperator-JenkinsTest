using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddedPlaceZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Zones",
                columns: new[] { "id", "function", "plcGateId", "temperatureRegime" },
                values: new object[,]
                {
                    { "PICK", "Pick", null, "Any" },
                    { "PLACE", "Place", null, "Any" }
                });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 9,
                column: "zoneId",
                value: "PICK");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 10,
                column: "zoneId",
                value: "PICK");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 15,
                column: "zoneId",
                value: "PICK");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 17,
                column: "zoneId",
                value: "PLACE");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "PICK_PLACE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Zones",
                columns: new[] { "id", "function", "plcGateId", "temperatureRegime" },
                values: new object[,]
                {
                    { "PICK_PLACE", "PickPlace", null, "Any" }
                });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 9,
                column: "zoneId",
                value: "PICK_PLACE");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 10,
                column: "zoneId",
                value: "PICK_PLACE");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 15,
                column: "zoneId",
                value: "PICK_PLACE");

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 17,
                column: "zoneId",
                value: "PICK_PLACE");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "PICK");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "PLACE");
        }
    }
}
