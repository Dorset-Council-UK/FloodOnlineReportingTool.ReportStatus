using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class SetSRIDOfGeomColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "geometry (point, 27700)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "geometry (point, 27700)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "geometry (point, 27700)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "geometry (point, 27700)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point, 27700)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point, 27700)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point, 27700)");

            migrationBuilder.AlterColumn<Point>(
                name: "Geom",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point, 27700)");

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
    }
}
