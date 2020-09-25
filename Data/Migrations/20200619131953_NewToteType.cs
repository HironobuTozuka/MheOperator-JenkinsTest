using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class NewToteType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "mhe",
                table: "ToteTypes",
                columns: new[] { "id", "toteHeight", "totePartitioning" },
                values: new object[] { 5, "unknown", "unknown" });
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121,
                column: "locationHeight",
                value: 180);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 95,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 86,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 87,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 76,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 32,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 24,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 151,
                column: "locationHeight",
                value: 180);
            
            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 141,
                column: "locationHeight",
                value: 180);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "ToteTypes",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121,
                column: "locationHeight",
                value: 170);

            migrationBuilder.UpdateData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131,
                column: "locationHeight",
                value: 170);
            
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
    }
}
