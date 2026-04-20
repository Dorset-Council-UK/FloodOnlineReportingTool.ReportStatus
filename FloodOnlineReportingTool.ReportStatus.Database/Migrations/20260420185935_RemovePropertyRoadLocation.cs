using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertyRoadLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportI~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropTable(
                name: "FloodLocationFloodReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodPropertyReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodRoadFloodReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodLocations",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodProperties",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodRoads",
                schema: "fortreportstatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSources",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSourceOrganisation",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportId_OrganisationId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSourceFloodCause",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.RenameColumn(
                name: "FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordType",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSources",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSourceOrganisation",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSourceFloodCause",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "EligibilityCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceId", "OrganisationId", "FloodAuthorityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportSourceId", "CauseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSources",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSourceOrganisation",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodReportSourceFloodCause",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropColumn(
                name: "RecordType",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                newName: "FloodReportId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                newName: "FloodReportId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSources",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                columns: new[] { "FloodReportId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSourceOrganisation",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportId", "FloodReportSourceId", "OrganisationId", "FloodAuthorityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodReportSourceFloodCause",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportId", "FloodReportSourceId", "CauseId" });

            migrationBuilder.CreateTable(
                name: "FloodLocations",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Geom = table.Column<Point>(type: "geometry (point, 27700)", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))", stored: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))", stored: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodProperties",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Geom = table.Column<Point>(type: "geometry (point, 27700)", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))", stored: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))", stored: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Uprn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodRoads",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Geom = table.Column<Point>(type: "geometry (point, 27700)", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))", stored: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))", stored: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Usrn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodRoads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodLocationFloodReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodLocationFloodReports", x => new { x.FloodLocationId, x.FloodReportId, x.FloodReportSourceId });
                    table.ForeignKey(
                        name: "FK_FloodLocationFloodReports_FloodLocations_FloodLocationId",
                        column: x => x.FloodLocationId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId_~",
                        columns: x => new { x.FloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodPropertyReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodPropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodPropertyReports", x => new { x.FloodPropertyId, x.FloodReportId, x.FloodReportSourceId });
                    table.ForeignKey(
                        name: "FK_FloodPropertyReports_FloodProperties_FloodPropertyId",
                        column: x => x.FloodPropertyId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId_Flood~",
                        columns: x => new { x.FloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodRoadFloodReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodRoadId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodRoadFloodReports", x => new { x.FloodRoadId, x.FloodReportId, x.FloodReportSourceId });
                    table.ForeignKey(
                        name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId_Floo~",
                        columns: x => new { x.FloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodRoadFloodReports_FloodRoads_FloodRoadId",
                        column: x => x.FloodRoadId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodRoads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportId_OrganisationId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportId", "OrganisationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportI~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportId", "FloodReportSourceId" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportId", "OrganisationId" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
