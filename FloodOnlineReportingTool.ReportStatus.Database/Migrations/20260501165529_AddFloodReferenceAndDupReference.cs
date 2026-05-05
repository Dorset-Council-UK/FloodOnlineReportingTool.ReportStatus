using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFloodReferenceAndDupReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodReportSourceDuplicateFloodReport",
                schema: "fortreportstatus");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FloodReportDuplicateReference",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousRecordStatus = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportSourceDuplicateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportDuplicateReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloodReportDuplicateReference_FloodReportSourceDuplicates_F~",
                        column: x => x.FloodReportSourceDuplicateId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReportSourceDuplicates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodReportDuplicateReference_FloodReports_FloodReportId",
                        column: x => x.FloodReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportDuplicateReference_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportDuplicateReference",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportDuplicateReference_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportDuplicateReference",
                column: "FloodReportSourceDuplicateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodReportDuplicateReference",
                schema: "fortreportstatus");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "fortreportstatus",
                table: "FloodReports");

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
                name: "IX_FloodReportSourceDuplicateFloodReport_FloodReportsId",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicateFloodReport",
                column: "FloodReportsId");
        }
    }
}
