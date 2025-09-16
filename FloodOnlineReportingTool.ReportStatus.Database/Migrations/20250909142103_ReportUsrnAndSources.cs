using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReportUsrnAndSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportDuplicates",
                schema: "fortreportstatus",
                table: "ReportDuplicates");

            migrationBuilder.DropIndex(
                name: "IX_ReportDuplicates_ReportId",
                schema: "fortreportstatus",
                table: "ReportDuplicates");

            migrationBuilder.AddColumn<long>(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "Reports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportDuplicates",
                schema: "fortreportstatus",
                table: "ReportDuplicates",
                columns: new[] { "ReportId", "Id" });

            migrationBuilder.CreateTable(
                name: "ReportSources",
                schema: "fortreportstatus",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSources", x => new { x.ReportId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReportSources_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportSources",
                schema: "fortreportstatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportDuplicates",
                schema: "fortreportstatus",
                table: "ReportDuplicates");

            migrationBuilder.DropColumn(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "Reports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportDuplicates",
                schema: "fortreportstatus",
                table: "ReportDuplicates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDuplicates_ReportId",
                schema: "fortreportstatus",
                table: "ReportDuplicates",
                column: "ReportId");
        }
    }
}
