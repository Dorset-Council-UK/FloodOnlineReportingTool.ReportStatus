using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAlternateKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");

            migrationBuilder.DropIndex(
                name: "IX_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                column: "Reference");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports",
                column: "Reference");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FloodEvents_Reference",
                schema: "fortreportstatus",
                table: "FloodEvents",
                column: "Reference");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FloodEvents_Reference",
                schema: "fortreportstatus",
                table: "FloodEvents");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceDuplicates_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSourceDuplicates",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReports_Reference",
                schema: "fortreportstatus",
                table: "FloodReports",
                column: "Reference",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceRef",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Reference");

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceRef",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Reference");
        }
    }
}
