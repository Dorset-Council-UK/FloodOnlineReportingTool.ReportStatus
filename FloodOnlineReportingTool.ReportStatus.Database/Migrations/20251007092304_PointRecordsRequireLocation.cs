using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class PointRecordsRequireLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "geometry (point)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodLocations",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)");
        }
    }
}
