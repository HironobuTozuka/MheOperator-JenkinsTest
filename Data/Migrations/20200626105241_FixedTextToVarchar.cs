using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class FixedTextToVarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "totePartitioning",
                schema: "mhe",
                table: "ToteTypes",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "toteHeight",
                schema: "mhe",
                table: "ToteTypes",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "toteBarcode",
                schema: "mhe",
                table: "Totes",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "rack",
                schema: "mhe",
                table: "Locations",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "totePartitioning",
                schema: "mhe",
                table: "ToteTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "toteHeight",
                schema: "mhe",
                table: "ToteTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "toteBarcode",
                schema: "mhe",
                table: "Totes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "rack",
                schema: "mhe",
                table: "Locations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
