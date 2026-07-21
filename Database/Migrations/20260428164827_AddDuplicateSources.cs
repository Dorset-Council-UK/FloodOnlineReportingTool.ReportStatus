using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDuplicateSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FloodReportSourceDuplicates",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    ViewUri = table.Column<string>(type: "text", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EligibilityCheckId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uprn = table.Column<long>(type: "bigint", nullable: true),
                    Usrn = table.Column<long>(type: "bigint", nullable: true),
                    Geom = table.Column<Point>(type: "geometry (point, 27700)", nullable: false),
                    Easting = table.Column<double>(type: "double precision", nullable: false, computedColumnSql: "ST_X(ST_Transform(\"Geom\", 27700))", stored: true),
                    Northing = table.Column<double>(type: "double precision", nullable: false, computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 27700))", stored: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))", stored: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))", stored: true),
                    ImpactStartUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ImpactDurationHours = table.Column<int>(type: "integer", nullable: true),
                    IsOnGoing = table.Column<bool>(type: "boolean", nullable: false),
                    IsUninhabitable = table.Column<bool>(type: "boolean", nullable: false),
                    VulnerableCount = table.Column<int>(type: "integer", nullable: true),
                    LocationDescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportSourceDuplicates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodReportSourceDuplicateFloodReport",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodReportSourceDuplicateId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportSourceDuplicateFloodReport", x => new { x.FloodReportSourceDuplicateId, x.FloodReportsId });
                    table.ForeignKey(
                        name: "FK_FloodReportSourceDuplicateFloodReport_FloodReportSourceDupl~",
                        column: x => x.FloodReportSourceDuplicateId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSourceDuplicates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodReportSourceDuplicateFloodReport_FloodReports_FloodRep~",
                        column: x => x.FloodReportsId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceDuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceDuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceDuplicateFloodReport_FloodReportsId",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicateFloodReport",
                column: "FloodReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceDuplicates_EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                column: "EligibilityCheckId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSourceDuplicates_Flo~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceDuplicateId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSourceDuplicates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSourceDuplicates_F~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceDuplicateId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSourceDuplicates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSourceDuplicates_Flo~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSourceDuplicates_F~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropTable(
                name: "FloodReportSourceDuplicateFloodReport",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodReportSourceDuplicates",
                schema: "fortreportstatus");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");
        }
    }
}
