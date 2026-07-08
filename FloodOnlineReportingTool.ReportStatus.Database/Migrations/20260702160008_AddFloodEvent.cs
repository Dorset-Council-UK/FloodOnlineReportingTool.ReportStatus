using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFloodEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FloodEvents",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    RecordStatus = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodStatus = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestigationStatus = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LinkedFloodReports = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    Geom = table.Column<MultiPolygon>(type: "geometry", nullable: true),
                    FloodStartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FloodEndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FloodCauses = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    Organisations = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    StatusFilter = table.Column<Guid[]>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodEvents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodEvents",
                schema: "fortreportstatus");
        }
    }
}
