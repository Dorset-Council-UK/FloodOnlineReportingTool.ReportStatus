using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodPropertyReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodPropertyReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "ImpactDurationHours",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "ImpactStartUtc",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReports",
                newName: "RecordStatus");

            migrationBuilder.AddColumn<string>(
                name: "BusName",
                schema: "fortreportstatus",
                table: "OutboxState",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "geometry (point, 27700)",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "ImpactDurationHours",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ImpactStartUtc",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnGoing",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUninhabitable",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocationDescription",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewUri",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VulnerableCount",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 27700))",
                stored: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<double>(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 27700))",
                stored: true);

            migrationBuilder.CreateTable(
                name: "EligibilityCheckFloodSource",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FloodReportSourceFloodReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityCheckFloodSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityCheckFloodSource_FloodReportSources_FloodReportS~",
                        columns: x => new { x.FloodReportSourceFloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" });
                });

            migrationBuilder.CreateTable(
                name: "EligibilityCheckOrganisation",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FloodAuthorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodAuthorityName = table.Column<string>(type: "text", nullable: false),
                    FloodReportSourceFloodReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityCheckOrganisation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityCheckOrganisation_FloodReportSources_FloodReport~",
                        columns: x => new { x.FloodReportSourceFloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_BusName_Created",
                schema: "fortreportstatus",
                table: "OutboxState",
                columns: new[] { "BusName", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityCheckFloodSource_FloodReportSourceFloodReportId_~",
                schema: "fortreportstatus",
                table: "EligibilityCheckFloodSource",
                columns: new[] { "FloodReportSourceFloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityCheckOrganisation_FloodReportSourceFloodReportId~",
                schema: "fortreportstatus",
                table: "EligibilityCheckOrganisation",
                columns: new[] { "FloodReportSourceFloodReportId", "FloodReportSourceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId1~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId1_Floo~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId1_Flo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId1~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId1_Floo~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId1_Flo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropTable(
                name: "EligibilityCheckFloodSource",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "EligibilityCheckOrganisation",
                schema: "fortreportstatus");

            migrationBuilder.DropIndex(
                name: "IX_OutboxState_BusName_Created",
                schema: "fortreportstatus",
                table: "OutboxState");

            migrationBuilder.DropIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodPropertyReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropColumn(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "BusName",
                schema: "fortreportstatus",
                table: "OutboxState");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropColumn(
                name: "EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "ImpactDurationHours",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "ImpactStartUtc",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "IsOnGoing",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "IsUninhabitable",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "LocationDescription",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "ViewUri",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "VulnerableCount",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "RecordStatus",
                schema: "fortreportstatus",
                table: "FloodReports",
                newName: "EligibilityCheckId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "geometry (point, 27700)",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "ImpactDurationHours",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ImpactStartUtc",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                column: "FloodReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                column: "FloodReportId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodPropertyReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                column: "FloodReportId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                column: "FloodReportId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
