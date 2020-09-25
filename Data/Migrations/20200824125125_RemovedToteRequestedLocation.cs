using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemovedToteRequestedLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Tote_RequestedLocation",
                schema: "mhe",
                table: "Totes");

            migrationBuilder.DropIndex(
                name: "IX_Totes_requestedLocationId",
                schema: "mhe",
                table: "Totes");

            migrationBuilder.DropColumn(
                name: "requestedLocationId",
                schema: "mhe",
                table: "Totes");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "requestedLocationId",
                schema: "mhe",
                table: "Totes",
                type: "integer",
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_Totes_requestedLocationId",
                schema: "mhe",
                table: "Totes",
                column: "requestedLocationId");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Tote_RequestedLocation",
                schema: "mhe",
                table: "Totes",
                column: "requestedLocationId",
                principalSchema: "mhe",
                principalTable: "Locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
