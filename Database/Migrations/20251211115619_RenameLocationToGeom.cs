using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameLocationToGeom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodRoads",
                newName: "Geom");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodReports",
                newName: "Geom");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodProperties",
                newName: "Geom");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodLocations",
                newName: "Geom");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodRoads",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodProperties",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodLocations",
                newName: "Location");
        }
    }
}
