using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_FloodReportId_EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                columns: new[] { "FloodReportId", "EligibilityCheckId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_FloodReportId_EligibilityCheckId",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "FloodReportId");
        }
    }
}
