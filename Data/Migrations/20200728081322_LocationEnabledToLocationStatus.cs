using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class LocationEnabledToLocationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "mhe",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "status",
                schema: "mhe",
                table: "Locations",
                nullable: false,
                defaultValue: "Enabled");
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 180 });
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 96,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 97,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 98,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 99,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "locationGroupId", "locationHeight" },
                values: new object[] { 1, 180 });
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "mhe",
                table: "Locations");

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "mhe",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1,
                column: "enabled",
                value: true);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 2,
                column: "enabled",
                value: true);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 3,
                column: "enabled",
                value: true);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 16,
                column: "enabled",
                value: true);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 17,
                column: "enabled",
                value: true);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 400 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 51,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 52,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 53,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 54,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 55,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 56,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 58,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 59,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 60,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 61,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 62,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 63,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 64,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 65,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 67,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 68,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 69,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 70,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 71,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 72,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 73,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 74,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 75,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 76,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 77,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 78,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 79,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 80,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 81,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 82,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 83,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 84,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 85,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 86,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 87,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 88,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 89,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 90,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 91,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 92,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 93,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 94,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 95,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 96,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 97,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 98,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 99,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "enabled", "locationGroupId", "locationHeight" },
                values: new object[] { true, 2, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 113,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 114,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 115,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 116,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 117,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 118,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 119,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 120,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 122,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 123,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 124,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 125,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 126,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 127,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 128,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 129,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 130,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 132,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 133,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 134,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 135,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 136,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 137,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 138,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 139,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 140,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 141,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 142,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 143,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 144,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 145,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 170 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 146,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 147,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 148,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 149,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 150,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 151,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 180 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 152,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 153,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 154,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 155,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 156,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 157,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 158,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 159,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 160,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 161,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 162,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 163,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 164,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 166,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 167,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 168,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 169,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 170,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 171,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 172,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 173,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 174,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 175,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 176,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 177,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 178,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 179,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 180,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 181,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 182,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 183,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 184,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 185,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 186,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 187,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 188,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 189,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 190,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 191,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 192,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 193,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 194,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 195,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 196,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 197,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 198,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 199,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 200,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 201,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 202,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 203,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 204,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 205,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 206,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 207,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 208,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 209,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 210,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 211,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 212,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 213,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 214,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 215,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 216,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 217,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 218,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 219,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 220,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 221,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 222,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 223,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 224,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 225,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 226,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 227,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 228,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 229,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 230,
                columns: new[] { "enabled", "locationHeight" },
                values: new object[] { true, 120 });

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 231,
                columns: new[] { "enabled", "isBackLocation", "locationHeight" },
                values: new object[] { true, true, 120 });
        }
    }
}
