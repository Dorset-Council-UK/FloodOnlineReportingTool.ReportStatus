using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFKConfusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId1~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId1_Floo~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId1_Flo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodRoadFloodReports",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodPropertyReports",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodPropertyReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodLocationFloodReports",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropColumn(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.RenameColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                newName: "FloodReportSourceId");

            migrationBuilder.RenameColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                newName: "FloodReportSourceId");

            migrationBuilder.RenameColumn(
                name: "FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                newName: "FloodReportSourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodRoadFloodReports",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodRoadId", "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodPropertyReports",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodPropertyId", "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodLocationFloodReports",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodLocationId", "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId_~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId_Flood~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId_Floo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId", "FloodReportSourceId" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId_~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId_Flood~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId_Floo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodRoadFloodReports",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodPropertyReports",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodPropertyReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloodLocationFloodReports",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.DropIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId_FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports");

            migrationBuilder.RenameColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                newName: "FloodReportId2");

            migrationBuilder.RenameColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                newName: "FloodReportId2");

            migrationBuilder.RenameColumn(
                name: "FloodReportSourceId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                newName: "FloodReportId2");

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FloodReportId1",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodRoadFloodReports",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodRoadId", "FloodReportId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodPropertyReports",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodPropertyId", "FloodReportId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloodLocationFloodReports",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodLocationId", "FloodReportId" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId1_FloodReportId2",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" });

            migrationBuilder.AddForeignKey(
                name: "FK_FloodLocationFloodReports_FloodReportSources_FloodReportId1~",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodPropertyReports_FloodReportSources_FloodReportId1_Floo~",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloodRoadFloodReports_FloodReportSources_FloodReportId1_Flo~",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                columns: new[] { "FloodReportId1", "FloodReportId2" },
                principalSchema: "fortreportstatus",
                principalTable: "FloodReportSources",
                principalColumns: new[] { "FloodReportId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
