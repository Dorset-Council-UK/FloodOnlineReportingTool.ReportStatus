using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EligibilityCheckFloodSource",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "EligibilityCheckOrganisation",
                schema: "fortreportstatus");

            migrationBuilder.CreateTable(
                name: "FloodReportSourceFloodCause",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CauseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportSourceFloodCause", x => new { x.FloodReportId, x.FloodReportSourceId, x.CauseId });
                    table.ForeignKey(
                        name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportI~",
                        columns: x => new { x.FloodReportId, x.FloodReportSourceId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodReportSourceOrganisation",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodAuthorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FloodAuthorityName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportSourceOrganisation", x => new { x.FloodReportId, x.FloodReportSourceId, x.OrganisationId, x.FloodAuthorityId });
                    table.ForeignKey(
                        name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                        columns: x => new { x.FloodReportId, x.OrganisationId },
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSources",
                        principalColumns: new[] { "FloodReportId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportId_OrganisationId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportId", "OrganisationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodReportSourceFloodCause",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodReportSourceOrganisation",
                schema: "fortreportstatus");

            migrationBuilder.CreateTable(
                name: "EligibilityCheckFloodSource",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceFloodReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
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
                    FloodAuthorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodAuthorityName = table.Column<string>(type: "text", nullable: false),
                    FloodReportSourceFloodReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    FloodReportSourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
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
                name: "IX_EligibilityCheckFloodSource_FloodReportSourceFloodReportId_~",
                schema: "fortreportstatus",
                table: "EligibilityCheckFloodSource",
                columns: new[] { "FloodReportSourceFloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityCheckOrganisation_FloodReportSourceFloodReportId~",
                schema: "fortreportstatus",
                table: "EligibilityCheckOrganisation",
                columns: new[] { "FloodReportSourceFloodReportId", "FloodReportSourceId" });
        }
    }
}
