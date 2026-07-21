using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDuplicateRelationships : Migration
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
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.AlterColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId_~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceDuplicateId", "OrganisationId", "FloodAuthorityId" },
                unique: true,
                filter: "\"FloodReportSourceDuplicateId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceId", "OrganisationId", "FloodAuthorityId" },
                unique: true,
                filter: "\"FloodReportSourceId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId_Ca~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportSourceDuplicateId", "CauseId" },
                unique: true,
                filter: "\"FloodReportSourceDuplicateId\" IS NOT NULL");

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
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId_~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId_Ca~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.DropIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause");

            migrationBuilder.AlterColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceDuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceOrganisation_FloodReportSourceId_Organisat~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                columns: new[] { "FloodReportSourceId", "OrganisationId", "FloodAuthorityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceDuplicateId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceDuplicateId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodReportSourceFloodCause_FloodReportSourceId_CauseId",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                columns: new[] { "FloodReportSourceId", "CauseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceFloodCause_FloodReportSources_FloodReportS~",
                schema: "fortreportstatus",
                table: "FloodReportSourceFloodCause",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodReportSourceOrganisation_FloodReportSources_FloodRepor~",
                schema: "fortreportstatus",
                table: "FloodReportSourceOrganisation",
                column: "FloodReportSourceId",
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
