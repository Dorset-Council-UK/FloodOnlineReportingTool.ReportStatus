using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class Check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComputedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                oldStored: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "numeric",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComputedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldComputedColumnSql: "ST_X(ST_Transform(\"Geom\", 4326))",
                oldStored: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "double precision",
                nullable: false,
                computedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldComputedColumnSql: "ST_Y(ST_Transform(\"Geom\", 4326))",
                oldStored: true);
        }
    }
}
