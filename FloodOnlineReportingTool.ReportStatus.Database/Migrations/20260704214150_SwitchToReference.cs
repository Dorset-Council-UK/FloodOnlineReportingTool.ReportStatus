using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToReference : Migration
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
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.AddColumn<string>(
                name: "FloodReportSourceRef",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloodReportSourceRef",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string[]>(
                name: "LinkedFloodReports",
                schema: "fortreportstatus",
                table: "FloodEvents",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources",
                column: "Reference");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceRef_Organisa~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceRef", "OrganisationId", "FloodAuthorityId" },
                unique: true,
                filter: "\"FloodReportSourceRef\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceRef_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportSourceRef", "CauseId" },
                unique: true,
                filter: "\"FloodReportSourceRef\" IS NOT NULL");

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

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FloodReportSources_Reference",
                schema: "fortreportstatus",
                table: "FloodReportSources");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceRef_Organisa~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceRef_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceRef",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropColumn(
                name: "FloodReportSourceRef",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

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

            migrationBuilder.AlterColumn<Guid[]>(
                name: "LinkedFloodReports",
                schema: "fortreportstatus",
                table: "FloodEvents",
                type: "uuid[]",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceId", "OrganisationId", "FloodAuthorityId" },
                unique: true,
                filter: "\"FloodReportSourceId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportSourceId", "CauseId" },
                unique: true,
                filter: "\"FloodReportSourceId\" IS NOT NULL");

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
    }
}
