using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class FirstSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "plcGateId",
                schema: "mhe",
                table: "Zones",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "LocationGroups",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "RackA_1" },
                    { 2, "RackA_2" },
                    { 3, "RackA_3" },
                    { 5, "RackB" }
                });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "ToteTypes",
                columns: new[] { "id", "toteHeight", "totePartitioning" },
                values: new object[,]
                {
                    { 2, "low", "tripartite" },
                    { 1, "low", "bipartite" },
                    { 3, "high", "bipartite" },
                    { 4, "high", "tripartite" }
                });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Zones",
                columns: new[] { "id", "function", "plcGateId", "temperatureRegime" },
                values: new object[,]
                {
                    { "PICK_PLACE", "PickPlace", null, "Any" },
                    { "LOADING_GATE", "LoadingGate", "LoadingGate", "Any" },
                    { "LOADING_GATE_CNV", "Conveyor", null, "Any" },
                    { "AMBIENT", "mhe", null, "Ambient" },
                    { "CHILL", "mhe", null, "Chill" },
                    { "STAGING", "Staging", null, "Ambient" },
                    { "CRANE", "Crane", null, "Any" },
                    { "CONVEYOR", "Conveyor", null, "Any" },
                    { "ORDER_GATE_1", "OrderGate", "OrderGate1", "Any" },
                    { "ORDER_GATE_2", "OrderGate", "OrderGate2", "Any" }
                });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Locations",
                columns: new[] { "id", "col", "enabled", "frontLocationId", "locationGroupId", "locationHeight", "plcId", "rack", "row", "zoneId" },
                values: new object[,]
                {
                    { 68, 7, true, null, 3, 170, "A1L0702", "A1", 2, "CHILL" },
                    { 95, 2, true, null, 1, 120, "A2L0210", "A2", 10, "CHILL" },
                    { 96, 3, true, null, 2, 170, "A2L0303", "A2", 3, "CHILL" },
                    { 97, 3, true, null, 2, 170, "A2L0304", "A2", 4, "CHILL" },
                    { 98, 3, true, null, 2, 120, "A2L0305", "A2", 5, "CHILL" },
                    { 99, 3, true, null, 2, 120, "A2L0306", "A2", 6, "CHILL" },
                    { 100, 3, true, null, 2, 120, "A2L0307", "A2", 7, "CHILL" },
                    { 101, 3, true, null, 2, 120, "A2L0308", "A2", 8, "CHILL" },
                    { 102, 3, true, null, 2, 120, "A2L0309", "A2", 9, "CHILL" },
                    { 103, 3, true, null, 2, 120, "A2L0310", "A2", 10, "CHILL" },
                    { 94, 2, true, null, 1, 120, "A2L0209", "A2", 9, "CHILL" },
                    { 104, 4, true, null, 2, 170, "A2L0403", "A2", 3, "CHILL" },
                    { 106, 4, true, null, 2, 120, "A2L0405", "A2", 5, "CHILL" },
                    { 107, 4, true, null, 2, 120, "A2L0406", "A2", 6, "CHILL" },
                    { 108, 4, true, null, 2, 120, "A2L0407", "A2", 7, "CHILL" },
                    { 109, 4, true, null, 2, 120, "A2L0408", "A2", 8, "CHILL" },
                    { 110, 4, true, null, 2, 120, "A2L0409", "A2", 9, "CHILL" },
                    { 111, 4, true, null, 2, 120, "A2L0410", "A2", 10, "CHILL" },
                    { 112, 5, true, null, 2, 120, "A2L0501", "A2", 1, "CHILL" },
                    { 113, 5, true, null, 2, 120, "A2L0502", "A2", 2, "CHILL" },
                    { 114, 5, true, null, 2, 120, "A2L0503", "A2", 3, "CHILL" },
                    { 105, 4, true, null, 2, 170, "A2L0404", "A2", 4, "CHILL" },
                    { 115, 5, true, null, 2, 170, "A2L0504", "A2", 4, "CHILL" },
                    { 93, 2, true, null, 1, 120, "A2L0208", "A2", 8, "CHILL" },
                    { 91, 2, true, null, 1, 120, "A2L0206", "A2", 6, "CHILL" },
                    { 71, 7, true, null, 3, 120, "A1L0705", "A1", 5, "CHILL" },
                    { 72, 7, true, null, 3, 120, "A1L0706", "A1", 6, "CHILL" },
                    { 73, 7, true, null, 3, 120, "A1L0707", "A1", 7, "CHILL" },
                    { 74, 7, true, null, 3, 120, "A1L0708", "A1", 8, "CHILL" },
                    { 75, 7, true, null, 3, 120, "A1L0709", "A1", 9, "CHILL" },
                    { 76, 7, true, null, 3, 120, "A1L0710", "A1", 10, "CHILL" },
                    { 77, 8, true, null, 3, 170, "A1L0801", "A1", 1, "CHILL" },
                    { 78, 8, true, null, 3, 170, "A1L0802", "A1", 2, "CHILL" },
                    { 79, 8, true, null, 3, 170, "A1L0803", "A1", 3, "CHILL" },
                    { 92, 2, true, null, 1, 120, "A2L0207", "A2", 7, "CHILL" },
                    { 80, 8, true, null, 3, 170, "A1L0804", "A1", 4, "CHILL" },
                    { 82, 8, true, null, 3, 120, "A1L0806", "A1", 6, "CHILL" },
                    { 83, 8, true, null, 3, 120, "A1L0807", "A1", 7, "CHILL" },
                    { 84, 8, true, null, 3, 120, "A1L0808", "A1", 8, "CHILL" },
                    { 85, 8, true, null, 3, 120, "A1L0809", "A1", 9, "CHILL" },
                    { 86, 8, true, null, 3, 120, "A1L0810", "A1", 10, "CHILL" },
                    { 87, 1, true, null, 1, 120, "A2L0110", "A2", 10, "CHILL" },
                    { 88, 2, true, null, 1, 170, "A2L0203", "A2", 3, "CHILL" },
                    { 89, 2, true, null, 1, 170, "A2L0204", "A2", 4, "CHILL" },
                    { 90, 2, true, null, 1, 120, "A2L0205", "A2", 5, "CHILL" },
                    { 81, 8, true, null, 3, 120, "A1L0805", "A1", 5, "CHILL" },
                    { 70, 7, true, null, 3, 170, "A1L0704", "A1", 4, "CHILL" },
                    { 116, 5, true, null, 2, 120, "A2L0505", "A2", 5, "CHILL" },
                    { 118, 5, true, null, 2, 120, "A2L0507", "A2", 7, "CHILL" },
                    { 143, 8, true, null, 3, 170, "A2L0802", "A2", 2, "CHILL" },
                    { 144, 8, true, null, 3, 170, "A2L0803", "A2", 3, "CHILL" },
                    { 145, 8, true, null, 3, 170, "A2L0804", "A2", 4, "CHILL" },
                    { 146, 8, true, null, 3, 120, "A2L0805", "A2", 5, "CHILL" },
                    { 147, 8, true, null, 3, 120, "A2L0806", "A2", 6, "CHILL" },
                    { 148, 8, true, null, 3, 120, "A2L0807", "A2", 7, "CHILL" },
                    { 149, 8, true, null, 3, 120, "A2L0808", "A2", 8, "CHILL" },
                    { 150, 8, true, null, 3, 120, "A2L0809", "A2", 9, "CHILL" },
                    { 151, 8, true, null, 3, 120, "A2L0810", "A2", 10, "CHILL" },
                    { 142, 8, true, null, 3, 170, "A2L0801", "A2", 1, "CHILL" },
                    { 170, 1, true, null, 5, 120, "B2L0101F", "B2", 1, "STAGING" },
                    { 174, 1, true, null, 5, 120, "B2L0104F", "B2", 4, "STAGING" },
                    { 176, 1, true, null, 5, 120, "B2L0105F", "B2", 5, "STAGING" },
                    { 190, 2, true, null, 5, 120, "B2L0201F", "B2", 1, "STAGING" },
                    { 192, 2, true, null, 5, 120, "B2L0202F", "B2", 2, "STAGING" },
                    { 194, 2, true, null, 5, 120, "B2L0203F", "B2", 3, "STAGING" },
                    { 196, 2, true, null, 5, 120, "B2L0204F", "B2", 4, "STAGING" },
                    { 198, 2, true, null, 5, 120, "B2L0205F", "B2", 5, "STAGING" },
                    { 212, 3, true, null, 5, 120, "B2L0301F", "B2", 1, "STAGING" },
                    { 214, 3, true, null, 5, 120, "B2L0303F", "B2", 3, "STAGING" },
                    { 172, 1, true, null, 5, 120, "B2L0103F", "B2", 3, "STAGING" },
                    { 117, 5, true, null, 2, 120, "A2L0506", "A2", 6, "CHILL" },
                    { 141, 7, true, null, 3, 120, "A2L0710", "A2", 10, "CHILL" },
                    { 139, 7, true, null, 3, 120, "A2L0708", "A2", 8, "CHILL" },
                    { 119, 5, true, null, 2, 120, "A2L0508", "A2", 8, "CHILL" },
                    { 120, 5, true, null, 2, 120, "A2L0509", "A2", 9, "CHILL" },
                    { 121, 5, true, null, 2, 120, "A2L0510", "A2", 10, "CHILL" },
                    { 122, 6, true, null, 2, 120, "A2L0601", "A2", 1, "CHILL" },
                    { 123, 6, true, null, 2, 120, "A2L0602", "A2", 2, "CHILL" },
                    { 124, 6, true, null, 2, 120, "A2L0603", "A2", 3, "CHILL" },
                    { 125, 6, true, null, 2, 170, "A2L0604", "A2", 4, "CHILL" },
                    { 126, 6, true, null, 2, 120, "A2L0605", "A2", 5, "CHILL" },
                    { 127, 6, true, null, 2, 120, "A2L0606", "A2", 6, "CHILL" },
                    { 140, 7, true, null, 3, 120, "A2L0709", "A2", 9, "CHILL" },
                    { 128, 6, true, null, 2, 120, "A2L0607", "A2", 7, "CHILL" },
                    { 130, 6, true, null, 2, 120, "A2L0609", "A2", 9, "CHILL" },
                    { 131, 6, true, null, 2, 120, "A2L0610", "A2", 10, "CHILL" },
                    { 132, 7, true, null, 3, 170, "A2L0701", "A2", 1, "CHILL" },
                    { 133, 7, true, null, 3, 170, "A2L0702", "A2", 2, "CHILL" },
                    { 134, 7, true, null, 3, 170, "A2L0703", "A2", 3, "CHILL" },
                    { 135, 7, true, null, 3, 170, "A2L0704", "A2", 4, "CHILL" },
                    { 136, 7, true, null, 3, 120, "A2L0705", "A2", 5, "CHILL" },
                    { 137, 7, true, null, 3, 120, "A2L0706", "A2", 6, "CHILL" },
                    { 138, 7, true, null, 3, 120, "A2L0707", "A2", 7, "CHILL" },
                    { 129, 6, true, null, 2, 120, "A2L0608", "A2", 8, "CHILL" },
                    { 69, 7, true, null, 3, 170, "A1L0703", "A1", 3, "CHILL" },
                    { 218, 3, true, null, 5, 120, "B2L0305F", "B2", 5, "STAGING" },
                    { 67, 7, true, null, 3, 170, "A1L0701", "A1", 1, "CHILL" },
                    { 154, 1, true, null, 5, 120, "B1L0110F", "B1", 10, "AMBIENT" },
                    { 156, 1, true, null, 5, 120, "B1L0111F", "B1", 11, "AMBIENT" },
                    { 158, 2, true, null, 5, 120, "B1L0209F", "B1", 9, "AMBIENT" },
                    { 160, 2, true, null, 5, 120, "B1L0210F", "B1", 10, "AMBIENT" },
                    { 162, 2, true, null, 5, 120, "B1L0211F", "B1", 11, "AMBIENT" },
                    { 164, 3, true, null, 5, 120, "B1L0309F", "B1", 9, "AMBIENT" },
                    { 166, 3, true, null, 5, 120, "B1L0310F", "B1", 10, "AMBIENT" },
                    { 168, 3, true, null, 5, 120, "B1L0311F", "B1", 11, "AMBIENT" },
                    { 178, 1, true, null, 5, 120, "B2L0106F", "B2", 6, "AMBIENT" },
                    { 152, 1, true, null, 5, 120, "B1L0109F", "B1", 9, "AMBIENT" },
                    { 180, 1, true, null, 5, 120, "B2L0107F", "B2", 7, "AMBIENT" },
                    { 184, 1, true, null, 5, 120, "B2L0109F", "B2", 9, "AMBIENT" },
                    { 186, 1, true, null, 5, 120, "B2L0110F", "B2", 10, "AMBIENT" },
                    { 188, 1, true, null, 5, 120, "B2L0111F", "B2", 11, "AMBIENT" },
                    { 200, 2, true, null, 5, 120, "B2L0206F", "B2", 6, "AMBIENT" },
                    { 202, 2, true, null, 5, 120, "B2L0207F", "B2", 7, "AMBIENT" },
                    { 204, 2, true, null, 5, 120, "B2L0208F", "B2", 8, "AMBIENT" },
                    { 206, 2, true, null, 5, 120, "B2L0209F", "B2", 9, "AMBIENT" },
                    { 208, 2, true, null, 5, 120, "B2L0210F", "B2", 10, "AMBIENT" },
                    { 210, 2, true, null, 5, 120, "B2L0211F", "B2", 11, "AMBIENT" },
                    { 182, 1, true, null, 5, 120, "B2L0108F", "B2", 8, "AMBIENT" },
                    { 220, 3, true, null, 5, 120, "B2L0306F", "B2", 6, "AMBIENT" },
                    { 21, 2, true, null, null, 170, "LOAD1_4", null, 1, "LOADING_GATE_CNV" },
                    { 18, 1, true, null, null, 170, "LOAD1_1", null, 1, "LOADING_GATE_CNV" },
                    { 4, 1, true, null, null, 400, "CNV1_1", null, 1, "CONVEYOR" },
                    { 5, 2, true, null, null, 400, "CNV1_2", null, 1, "CONVEYOR" },
                    { 6, 3, true, null, null, 400, "CNV1_3", null, 1, "CONVEYOR" },
                    { 7, 4, true, null, null, 400, "CNV1_4", null, 1, "CONVEYOR" },
                    { 8, 5, true, null, null, 400, "CNV1_5", null, 1, "CONVEYOR" },
                    { 11, 1, true, null, null, 400, "CNV2_1", null, 3, "CONVEYOR" },
                    { 12, 2, true, null, null, 400, "CNV2_2", null, 3, "CONVEYOR" },
                    { 13, 3, true, null, null, 400, "CNV2_3", null, 3, "CONVEYOR" },
                    { 14, 4, true, null, null, 400, "CNV2_4", null, 3, "CONVEYOR" },
                    { 20, 2, true, null, null, 170, "LOAD1_3", null, 2, "LOADING_GATE_CNV" },
                    { 16, null, false, null, null, 400, "CNV1", null, null, "CONVEYOR" },
                    { 2, null, false, null, null, 400, "CA_P2", "A", null, "CRANE" },
                    { 3, null, false, null, null, 400, "CB_P1", "B", null, "CRANE" },
                    { 9, 3, true, null, null, 400, "CNV3_2", null, 2, "PICK_PLACE" },
                    { 10, 4, true, null, null, 400, "CNV4_2", null, 2, "PICK_PLACE" },
                    { 15, 5, true, null, null, 400, "CNV2_5", null, 3, "PICK_PLACE" },
                    { 17, 6, false, null, null, 400, "RPP1", null, 1, "PICK_PLACE" },
                    { 22, 1, true, null, null, 400, "ORDER1", null, null, "ORDER_GATE_1" },
                    { 23, 2, true, null, null, 400, "ORDER2", null, null, "ORDER_GATE_2" },
                    { 19, 1, true, null, null, 170, "LOAD1_2", null, 2, "LOADING_GATE" },
                    { 1, null, false, null, null, 400, "CA_P1", "A", null, "CRANE" },
                    { 222, 3, true, null, 5, 120, "B2L0307F", "B2", 7, "AMBIENT" },
                    { 224, 3, true, null, 5, 120, "B2L0308F", "B2", 8, "AMBIENT" },
                    { 226, 3, true, null, 5, 120, "B2L0309F", "B2", 9, "AMBIENT" },
                    { 47, 4, true, null, 2, 120, "A1L0409", "A1", 9, "CHILL" },
                    { 48, 4, true, null, 2, 120, "A1L0410", "A1", 10, "CHILL" },
                    { 49, 5, true, null, 2, 120, "A1L0501", "A1", 1, "CHILL" },
                    { 50, 5, true, null, 2, 120, "A1L0503", "A1", 3, "CHILL" },
                    { 51, 5, true, null, 2, 170, "A1L0504", "A1", 4, "CHILL" },
                    { 52, 5, true, null, 2, 120, "A1L0505", "A1", 5, "CHILL" },
                    { 53, 5, true, null, 2, 120, "A1L0506", "A1", 6, "CHILL" },
                    { 54, 5, true, null, 2, 120, "A1L0507", "A1", 7, "CHILL" },
                    { 55, 5, true, null, 2, 120, "A1L0508", "A1", 8, "CHILL" },
                    { 46, 4, true, null, 2, 120, "A1L0408", "A1", 8, "CHILL" },
                    { 56, 5, true, null, 2, 120, "A1L0509", "A1", 9, "CHILL" },
                    { 58, 6, true, null, 2, 120, "A1L0601", "A1", 1, "CHILL" },
                    { 59, 6, true, null, 2, 120, "A1L0603", "A1", 3, "CHILL" },
                    { 60, 6, true, null, 2, 170, "A1L0604", "A1", 4, "CHILL" },
                    { 61, 6, true, null, 2, 120, "A1L0605", "A1", 5, "CHILL" },
                    { 62, 6, true, null, 2, 120, "A1L0606", "A1", 6, "CHILL" },
                    { 63, 6, true, null, 2, 120, "A1L0607", "A1", 7, "CHILL" },
                    { 64, 6, true, null, 2, 120, "A1L0608", "A1", 8, "CHILL" },
                    { 65, 6, true, null, 2, 120, "A1L0609", "A1", 9, "CHILL" },
                    { 66, 6, true, null, 2, 120, "A1L0610", "A1", 10, "CHILL" },
                    { 57, 5, true, null, 2, 120, "A1L0510", "A1", 10, "CHILL" },
                    { 45, 4, true, null, 2, 120, "A1L0407", "A1", 7, "CHILL" },
                    { 44, 4, true, null, 2, 120, "A1L0406", "A1", 6, "CHILL" },
                    { 43, 4, true, null, 2, 120, "A1L0405", "A1", 5, "CHILL" },
                    { 228, 3, true, null, 5, 120, "B2L0310F", "B2", 10, "AMBIENT" },
                    { 230, 3, true, null, 5, 120, "B2L0311F", "B2", 11, "AMBIENT" },
                    { 24, 1, true, null, 1, 120, "A1L0110", "A1", 10, "CHILL" },
                    { 25, 2, true, null, 1, 170, "A1L0203", "A1", 3, "CHILL" },
                    { 26, 2, true, null, 1, 170, "A1L0204", "A1", 4, "CHILL" },
                    { 27, 2, true, null, 1, 120, "A1L0205", "A1", 5, "CHILL" },
                    { 28, 2, true, null, 1, 120, "A1L0206", "A1", 6, "CHILL" },
                    { 29, 2, true, null, 1, 120, "A1L0207", "A1", 7, "CHILL" },
                    { 30, 2, true, null, 1, 120, "A1L0208", "A1", 8, "CHILL" },
                    { 31, 2, true, null, 1, 120, "A1L0209", "A1", 9, "CHILL" },
                    { 32, 2, true, null, 1, 120, "A1L0210", "A1", 10, "CHILL" },
                    { 33, 3, true, null, 2, 170, "A1L0303", "A1", 3, "CHILL" },
                    { 34, 3, true, null, 2, 170, "A1L0304", "A1", 4, "CHILL" },
                    { 35, 3, true, null, 2, 120, "A1L0305", "A1", 5, "CHILL" },
                    { 36, 3, true, null, 2, 120, "A1L0306", "A1", 6, "CHILL" },
                    { 37, 3, true, null, 2, 120, "A1L0307", "A1", 7, "CHILL" },
                    { 38, 3, true, null, 2, 120, "A1L0308", "A1", 8, "CHILL" },
                    { 39, 3, true, null, 2, 120, "A1L0309", "A1", 9, "CHILL" },
                    { 40, 3, true, null, 2, 120, "A1L0310", "A1", 10, "CHILL" },
                    { 41, 4, true, null, 2, 170, "A1L0403", "A1", 3, "CHILL" },
                    { 42, 4, true, null, 2, 170, "A1L0404", "A1", 4, "CHILL" },
                    { 216, 3, true, null, 5, 120, "B2L0304F", "B2", 4, "STAGING" }
                });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Routes",
                columns: new[] { "id", "deviceId", "isDefaultRoute", "locationId", "locationTypeId", "routeCost", "routedLocationId", "routedLocationTypeId" },
                values: new object[] { 44, "CB_P1", true, null, 5, 1, null, 5 });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Locations",
                columns: new[] { "id", "col", "enabled", "frontLocationId", "isBackLocation", "locationGroupId", "locationHeight", "plcId", "rack", "row", "zoneId" },
                values: new object[,]
                {
                    { 219, 3, true, 218, true, 5, 120, "B2L0305B", "B2", 5, "AMBIENT" },
                    { 203, 2, true, 202, true, 5, 120, "B2L0207B", "B2", 7, "AMBIENT" },
                    { 201, 2, true, 200, true, 5, 120, "B2L0206B", "B2", 6, "AMBIENT" },
                    { 189, 1, true, 188, true, 5, 120, "B2L0111B", "B2", 11, "AMBIENT" },
                    { 187, 1, true, 186, true, 5, 120, "B2L0110B", "B2", 10, "AMBIENT" },
                    { 185, 1, true, 184, true, 5, 120, "B2L0109B", "B2", 9, "AMBIENT" },
                    { 183, 1, true, 182, true, 5, 120, "B2L0108B", "B2", 8, "AMBIENT" },
                    { 181, 1, true, 180, true, 5, 120, "B2L0107B", "B2", 7, "AMBIENT" },
                    { 205, 2, true, 204, true, 5, 120, "B2L0208B", "B2", 8, "AMBIENT" },
                    { 179, 1, true, 178, true, 5, 120, "B2L0106B", "B2", 6, "AMBIENT" },
                    { 165, 3, true, 164, true, 5, 120, "B1L0309B", "B1", 9, "AMBIENT" },
                    { 163, 2, true, 162, true, 5, 120, "B1L0211B", "B1", 11, "AMBIENT" },
                    { 161, 2, true, 160, true, 5, 120, "B1L0210B", "B1", 10, "AMBIENT" },
                    { 159, 2, true, 158, true, 5, 120, "B1L0209B", "B1", 9, "AMBIENT" },
                    { 157, 1, true, 156, true, 5, 120, "B1L0111B", "B1", 11, "AMBIENT" },
                    { 155, 1, true, 154, true, 5, 120, "B1L0110B", "B1", 10, "AMBIENT" },
                    { 153, 1, true, 152, true, 5, 120, "B1L0109B", "B1", 9, "AMBIENT" },
                    { 169, 3, true, 168, true, 5, 120, "B1L0311B", "B1", 11, "AMBIENT" },
                    { 207, 2, true, 206, true, 5, 120, "B2L0209B", "B2", 9, "AMBIENT" },
                    { 167, 3, true, 166, true, 5, 120, "B1L0310B", "B1", 10, "AMBIENT" },
                    { 211, 2, true, 210, true, 5, 120, "B2L0211B", "B2", 11, "AMBIENT" },
                    { 209, 2, true, 208, true, 5, 120, "B2L0210B", "B2", 10, "AMBIENT" },
                    { 215, 3, true, 214, true, 5, 120, "B2L0303B", "B2", 3, "AMBIENT" },
                    { 213, 3, true, 212, true, 5, 120, "B2L0301B", "B2", 1, "AMBIENT" },
                    { 199, 2, true, 198, true, 5, 120, "B2L0205B", "B2", 5, "AMBIENT" },
                    { 197, 2, true, 196, true, 5, 120, "B2L0204B", "B2", 4, "AMBIENT" },
                    { 195, 2, true, 194, true, 5, 120, "B2L0203B", "B2", 3, "AMBIENT" },
                    { 193, 2, true, 192, true, 5, 120, "B2L0202B", "B2", 2, "AMBIENT" },
                    { 191, 2, true, 190, true, 5, 120, "B2L0201B", "B2", 1, "AMBIENT" },
                    { 177, 1, true, 176, true, 5, 120, "B2L0105B", "B2", 5, "AMBIENT" },
                    { 217, 3, true, 216, true, 5, 120, "B2L0304B", "B2", 4, "AMBIENT" },
                    { 173, 1, true, 172, true, 5, 120, "B2L0103B", "B2", 3, "AMBIENT" },
                    { 171, 1, true, 170, true, 5, 120, "B2L0101B", "B2", 1, "AMBIENT" },
                    { 231, 3, true, 230, true, 5, 120, "B2L0311B", "B2", 11, "AMBIENT" },
                    { 229, 3, true, 228, true, 5, 120, "B2L0310B", "B2", 10, "AMBIENT" },
                    { 227, 3, true, 226, true, 5, 120, "B2L0309B", "B2", 9, "AMBIENT" },
                    { 225, 3, true, 224, true, 5, 120, "B2L0308B", "B2", 8, "AMBIENT" },
                    { 223, 3, true, 222, true, 5, 120, "B2L0307B", "B2", 7, "AMBIENT" },
                    { 175, 1, true, 174, true, 5, 120, "B2L0104B", "B2", 4, "AMBIENT" },
                    { 221, 3, true, 220, true, 5, 120, "B2L0306B", "B2", 6, "AMBIENT" }
                });

            migrationBuilder.InsertData(
                schema: "mhe",
                table: "Routes",
                columns: new[] { "id", "deviceId", "isDefaultRoute", "locationId", "locationTypeId", "routeCost", "routedLocationId", "routedLocationTypeId" },
                values: new object[,]
                {
                    { 60, "CB_P1", true, 8, null, 1, 23, null },
                    { 20, "CA_P1", true, null, 2, 1, 18, null },
                    { 19, "CA_P1", true, null, 1, 1, 18, null },
                    { 18, "CA_P1", false, 11, null, 1, 18, null },
                    { 67, "CB_P1", false, 3, null, 1, 23, null },
                    { 77, "CB_P1", true, 23, null, 1, 17, null },
                    { 75, "CB_P1", true, 17, null, 1, 23, null },
                    { 21, "CA_P2", true, null, 2, 1, 18, null },
                    { 62, "CB_P1", true, null, 5, 1, 23, null },
                    { 17, "CA_P1", false, 12, null, 1, 18, null },
                    { 22, "CA_P2", true, null, 3, 1, 18, null },
                    { 23, "CA_P1", true, 21, null, 1, null, 1 },
                    { 42, "CA_P2", false, 2, null, 1, 18, null },
                    { 2, "PLC", true, 18, null, 1, 19, null },
                    { 1, "LOAD1", true, 19, null, 1, 20, null },
                    { 24, "CA_P1", true, 21, null, 1, null, 2 },
                    { 25, "CA_P2", true, 21, null, 1, null, 2 },
                    { 26, "CA_P2", true, 21, null, 1, null, 3 },
                    { 27, "CA_P1", true, 21, null, 1, 16, null },
                    { 28, "CA_P1", false, 21, null, 1, 1, null },
                    { 43, "CA_P1", false, 21, null, 1, 18, null },
                    { 3, "PLC", true, 20, null, 1, 21, null },
                    { 41, "CA_P1", false, 1, null, 1, 18, null },
                    { 32, "CA_P2", false, 21, null, 1, 2, null },
                    { 58, "CB_P1", true, 15, null, 1, 23, null },
                    { 54, "CB_P1", true, 23, null, 1, 15, null },
                    { 33, "CA_P2", false, 2, null, 1, 16, null },
                    { 40, "CA_P1", false, 1, null, 1, 1, null },
                    { 37, "CA_P1", false, 12, null, 1, 1, null },
                    { 36, "CA_P1", false, 11, null, 1, 1, null },
                    { 31, "CA_P1", true, 1, null, 1, null, 2 },
                    { 30, "CA_P1", true, 1, null, 1, null, 1 },
                    { 29, "CA_P1", false, 1, null, 1, 16, null },
                    { 16, "CA_P2", false, 11, null, 1, 16, null },
                    { 15, "CA_P1", false, 12, null, 1, 16, null },
                    { 14, "CA_P1", false, 11, null, 1, 16, null },
                    { 7, "CA_P2", true, null, 3, 1, 16, null },
                    { 6, "CA_P2", true, null, 2, 1, 16, null },
                    { 5, "CA_P1", true, null, 2, 1, 16, null },
                    { 34, "CA_P2", true, 2, null, 1, null, 3 },
                    { 4, "CA_P1", true, null, 1, 1, 16, null },
                    { 85, "CNV", true, 14, null, 1, 13, null },
                    { 83, "CNV", true, 13, null, 1, 12, null },
                    { 9, "CA_P1", false, 12, null, 1, null, 2 },
                    { 8, "CA_P1", false, 12, null, 1, null, 1 },
                    { 84, "CNV", false, 12, null, 1, 11, null },
                    { 13, "CA_P2", false, 11, null, 1, null, 3 },
                    { 12, "CA_P2", false, 11, null, 1, null, 2 },
                    { 11, "CA_P1", false, 11, null, 1, null, 2 },
                    { 10, "CA_P1", false, 11, null, 1, null, 1 },
                    { 48, "CB_P1", true, 8, null, 1, null, 5 },
                    { 46, "CB_P1", true, null, 5, 1, 8, null },
                    { 87, "CNV", false, 8, null, 1, 7, null },
                    { 81, "CNV", true, 7, null, 1, 8, null },
                    { 78, "CNV", false, 16, null, 1, 6, null },
                    { 56, "CB_P1", true, 23, null, 1, 8, null },
                    { 35, "CA_P2", true, 2, null, 1, null, 2 },
                    { 39, "CA_P2", false, 2, null, 1, 2, null },
                    { 52, "CB_P1", false, 23, null, 1, null, 5 },
                    { 76, "CB_P1", true, 22, null, 1, 17, null },
                    { 74, "CB_P1", true, 17, null, 1, 22, null },
                    { 66, "CB_P1", false, 3, null, 1, 22, null },
                    { 61, "CB_P1", true, null, 5, 1, 22, null },
                    { 59, "CB_P1", true, 8, null, 1, 22, null },
                    { 57, "CB_P1", true, 15, null, 1, 22, null },
                    { 55, "CB_P1", true, 22, null, 1, 8, null },
                    { 53, "CB_P1", true, 22, null, 1, 15, null },
                    { 51, "CB_P1", false, 22, null, 1, null, 5 },
                    { 73, "CB_P1", true, 17, null, 1, 15, null },
                    { 72, "CB_P1", true, 15, null, 1, 17, null },
                    { 71, "CB_P1", true, 8, null, 1, 17, null },
                    { 38, "CA_P2", false, 11, null, 1, 2, null },
                    { 70, "CB_P1", true, null, 5, 1, 17, null },
                    { 65, "CB_P1", false, 3, null, 1, 15, null },
                    { 50, "CB_P1", true, 15, null, 1, 8, null },
                    { 49, "CB_P1", true, 8, null, 1, 15, null },
                    { 47, "CB_P1", true, 15, null, 1, null, 5 },
                    { 45, "CB_P1", true, null, 5, 1, 15, null },
                    { 86, "CNV", false, 15, null, 1, 14, null },
                    { 89, "CNV", true, 10, null, 1, 14, null },
                    { 82, "CNV", false, 7, null, 1, 10, null },
                    { 88, "CNV", true, 9, null, 1, 13, null },
                    { 80, "CNV", false, 6, null, 1, 9, null },
                    { 68, "CB_P1", true, 3, null, 1, null, 5 },
                    { 64, "CB_P1", false, 3, null, 1, 8, null },
                    { 63, "CB_P1", false, 3, null, 1, 3, null },
                    { 69, "CB_P1", true, 17, null, 1, null, 5 },
                    { 79, "CNV", true, 6, null, 1, 7, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Routes",
                keyColumn: "id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "ToteTypes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "ToteTypes",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "ToteTypes",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "ToteTypes",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "LocationGroups",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "LocationGroups",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "LocationGroups",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Locations",
                keyColumn: "id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "CHILL");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "LocationGroups",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "AMBIENT");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "CONVEYOR");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "CRANE");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "LOADING_GATE");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "LOADING_GATE_CNV");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "ORDER_GATE_1");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "ORDER_GATE_2");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "PICK_PLACE");

            migrationBuilder.DeleteData(
                schema: "mhe",
                table: "Zones",
                keyColumn: "id",
                keyValue: "STAGING");

            migrationBuilder.AlterColumn<string>(
                name: "plcGateId",
                schema: "mhe",
                table: "Zones",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);
        }
    }
}
