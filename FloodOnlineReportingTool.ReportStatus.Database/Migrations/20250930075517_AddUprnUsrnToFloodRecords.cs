using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUprnUsrnToFloodRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodRoads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodProperties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usrn",
                schema: "fortreportstatus",
                table: "FloodRoads");

            migrationBuilder.DropColumn(
                name: "Uprn",
                schema: "fortreportstatus",
                table: "FloodProperties");
        }
    }
}
