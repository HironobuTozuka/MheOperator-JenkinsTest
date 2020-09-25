using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class adddedTechnicalZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Zones",
                columns: new[] { "id", "function", "plcGateId", "temperatureRegime" },
                values: new object[] { "TECHNICAL", "Technical", null, "Any" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 170, "TECHNICAL" });
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 95,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 86,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 87,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 76,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 32,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 24,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 151,
                column: "locationHeight",
                value: 170);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 141,
                column: "locationHeight",
                value: 170);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "locationHeight", "zoneId" },
                values: new object[] { 120, "CHILL" });
            
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "TECHNICAL");
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 95,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 86,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 87,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 76,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 32,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 24,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 151,
                column: "locationHeight",
                value: 120);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 141,
                column: "locationHeight",
                value: 120);
        }
    }
}
