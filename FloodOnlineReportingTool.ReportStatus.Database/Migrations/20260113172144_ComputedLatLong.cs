using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class ComputedLatLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true);

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

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodRoads");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodRoads");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodProperties");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodProperties");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodLocations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodLocations");
        }
    }
}
