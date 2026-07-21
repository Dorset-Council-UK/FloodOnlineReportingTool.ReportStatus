using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class FloodReportEastingNorthingToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new Location column (non-nullable with a default to satisfy constraint temporarily)
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "geometry (point)",
                nullable: false,
                defaultValue: new Point(0, 0) { SRID = 27700 });

            // Migrate existing Easting/Northing data to Location using PostGIS ST_SetSRID and ST_MakePoint
            migrationBuilder.Sql(@"
                UPDATE fortreportstatus.""FloodReports""
                SET ""Location"" = ST_SetSRID(ST_MakePoint(""Easting"", ""Northing""), 27700);
            ");

            // Drop the old columns
            migrationBuilder.DropColumn(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropColumn(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add back the old columns
            migrationBuilder.AddColumn<double>(
                name: "Easting",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Northing",
                schema: "fortreportstatus",
                table: "FloodReports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            // Migrate Location data back to Easting/Northing using PostGIS ST_X and ST_Y
            migrationBuilder.Sql(@"
                UPDATE fortreportstatus.""FloodReports""
                SET ""Easting"" = ST_X(""Location""),
                    ""Northing"" = ST_Y(""Location"");
            ");

            // Drop the Location column
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "fortreportstatus",
                table: "FloodReports");
        }
    }
}
