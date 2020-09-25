using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mhe");

            migrationBuilder.CreateTable(
                name: "LocationGroups",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_LocationGroups", x => x.id); });

            migrationBuilder.CreateTable(
                name: "Zones",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 255, nullable: false),
                    temperatureRegime = table.Column<string>(maxLength: 255, nullable: false),
                    function = table.Column<string>(maxLength: 255, nullable: false),
                    plcGateId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Zones", x => x.id); });

            migrationBuilder.CreateTable(
                name: "ToteTypes",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    totePartitioning = table.Column<string>(nullable: false),
                    toteHeight = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ToteTypes", x => x.id); });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    locationGroupId = table.Column<int>(nullable: true),
                    plcId = table.Column<string>(maxLength: 15, nullable: false),
                    locationHeight = table.Column<int>(nullable: false, defaultValue: 0),
                    isBackLocation = table.Column<bool>(nullable: false, defaultValue: false),
                    frontLocationId = table.Column<int>(nullable: true),
                    rack = table.Column<string>(nullable: true),
                    col = table.Column<int>(nullable: true),
                    row = table.Column<int>(nullable: true),
                    enabled = table.Column<bool>(nullable: false),
                    zoneId = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                    table.ForeignKey(
                        name: "ForeignKey_Location_FrontLocation",
                        column: x => x.frontLocationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_Location_LocationGroup",
                        column: x => x.locationGroupId,
                        principalSchema: "mhe",
                        principalTable: "LocationGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Zones_zoneId",
                        column: x => x.zoneId,
                        principalSchema: "mhe",
                        principalTable: "Zones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    deviceId = table.Column<string>(maxLength: 50, nullable: false),
                    locationTypeId = table.Column<int>(nullable: true),
                    locationId = table.Column<int>(nullable: true),
                    routedLocationTypeId = table.Column<int>(nullable: true),
                    routedLocationId = table.Column<int>(nullable: true),
                    isDefaultRoute = table.Column<bool>(nullable: false),
                    routeCost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.id);
                    table.CheckConstraint("Constraint_OneSourceNotNull",
                        "NOT ((\"Routes\".\"locationId\" IS NULL) AND (\"Routes\".\"locationTypeId\" IS NULL))");
                    table.CheckConstraint("Constraint_OneDestNotNull",
                        "NOT ((\"Routes\".\"routedLocationId\" IS NULL) AND (\"Routes\".\"routedLocationTypeId\" IS NULL))");
                    table.ForeignKey(
                        name: "ForeignKey_Location",
                        column: x => x.locationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_LocationType",
                        column: x => x.locationTypeId,
                        principalSchema: "mhe",
                        principalTable: "LocationGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_RoutedLocation",
                        column: x => x.routedLocationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_RoutedLocationType",
                        column: x => x.routedLocationTypeId,
                        principalSchema: "mhe",
                        principalTable: "LocationGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Totes",
                schema: "mhe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    toteBarcode = table.Column<string>(nullable: false),
                    locationId = table.Column<int>(nullable: true),
                    requestedLocationId = table.Column<int>(nullable: true),
                    storageLocationId = table.Column<int>(nullable: false),
                    typeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totes", x => x.id);
                    table.ForeignKey(
                        name: "ForeignKey_Tote_Location",
                        column: x => x.locationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_Tote_RequestedLocation",
                        column: x => x.requestedLocationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Totes_Locations_storageLocationId",
                        column: x => x.storageLocationId,
                        principalSchema: "mhe",
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_Tote_ToteType",
                        column: x => x.typeId,
                        principalSchema: "mhe",
                        principalTable: "ToteTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_name",
                schema: "mhe",
                table: "LocationGroups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_frontLocationId",
                schema: "mhe",
                table: "Locations",
                column: "frontLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_locationGroupId",
                schema: "mhe",
                table: "Locations",
                column: "locationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_zoneId",
                schema: "mhe",
                table: "Locations",
                column: "zoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_plcId",
                schema: "mhe",
                table: "Locations",
                column: "plcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_locationId",
                schema: "mhe",
                table: "Routes",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_locationTypeId",
                schema: "mhe",
                table: "Routes",
                column: "locationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_routedLocationId",
                schema: "mhe",
                table: "Routes",
                column: "routedLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_routedLocationTypeId",
                schema: "mhe",
                table: "Routes",
                column: "routedLocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Totes_locationId",
                schema: "mhe",
                table: "Totes",
                column: "locationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Totes_requestedLocationId",
                schema: "mhe",
                table: "Totes",
                column: "requestedLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Totes_storageLocationId",
                schema: "mhe",
                table: "Totes",
                column: "storageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Totes_toteBarcode",
                schema: "mhe",
                table: "Totes",
                column: "toteBarcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Totes_typeId",
                schema: "mhe",
                table: "Totes",
                column: "typeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}