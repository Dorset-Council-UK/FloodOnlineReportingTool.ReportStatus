using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGeomToFloodReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "geometry (point, 27700)",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 27700))",
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

            migrationBuilder.AddColumn<double>(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 27700))",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports");
        }
    }
}
