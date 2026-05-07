using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddContactAndInvestigationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "ContactRecordTypes",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<bool>(
                name: "HasContacts",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInvestigation",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int[]>(
                name: "ContactRecordTypes",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<bool>(
                name: "HasContacts",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInvestigation",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactRecordTypes",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "HasContacts",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "HasInvestigation",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropColumn(
                name: "ContactRecordTypes",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");

            migrationBuilder.DropColumn(
                name: "HasContacts",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");

            migrationBuilder.DropColumn(
                name: "HasInvestigation",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");
        }
    }
}
