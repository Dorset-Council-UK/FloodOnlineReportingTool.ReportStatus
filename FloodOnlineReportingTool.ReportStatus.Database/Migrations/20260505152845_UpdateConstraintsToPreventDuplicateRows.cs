using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConstraintsToPreventDuplicateRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports",
                column: "Reference",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");

            migrationBuilder.DropIndex(
                name: "IX_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports");
        }
    }
}
