using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FloodOnlineReportingTool.ReportStatus.Database.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabaseDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fortreportstatus");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "FloodLocations",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<Point>(type: "geometry (point)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodProperties",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<Point>(type: "geometry (point)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EligibilityCheckId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uprn = table.Column<long>(type: "bigint", nullable: true),
                    Usrn = table.Column<long>(type: "bigint", nullable: true),
                    Easting = table.Column<double>(type: "double precision", nullable: false),
                    Northing = table.Column<double>(type: "double precision", nullable: false),
                    ImpactStartUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ImpactDurationHours = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FloodRoads",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<Point>(type: "geometry (point)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodRoads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboxState",
                schema: "fortreportstatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxState",
                schema: "fortreportstatus",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxState", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "FloodLocationFloodReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodLocationFloodReports", x => new { x.FloodLocationId, x.FloodReportId });
                    table.ForeignKey(
                        name: "FK_FloodLocationFloodReports_FloodLocations_FloodLocationId",
                        column: x => x.FloodLocationId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodLocationFloodReports_FloodReports_FloodReportId",
                        column: x => x.FloodReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodPropertyReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodPropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodPropertyReports", x => new { x.FloodPropertyId, x.FloodReportId });
                    table.ForeignKey(
                        name: "FK_FloodPropertyReports_FloodProperties_FloodPropertyId",
                        column: x => x.FloodPropertyId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodPropertyReports_FloodReports_FloodReportId",
                        column: x => x.FloodReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodReportSources",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodReportSources", x => new { x.FloodReportId, x.SourceId });
                    table.ForeignKey(
                        name: "FK_FloodReportSources_FloodReports_FloodReportId",
                        column: x => x.FloodReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodRoadFloodReports",
                schema: "fortreportstatus",
                columns: table => new
                {
                    FloodRoadId = table.Column<Guid>(type: "uuid", nullable: false),
                    FloodReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedByUserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmedUserName = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodRoadFloodReports", x => new { x.FloodRoadId, x.FloodReportId });
                    table.ForeignKey(
                        name: "FK_FloodRoadFloodReports_FloodReports_FloodReportId",
                        column: x => x.FloodReportId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloodRoadFloodReports_FloodRoads_FloodRoadId",
                        column: x => x.FloodRoadId,
                        principalSchema: "fortreportstatus",
                        principalTable: "FloodRoads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "fortreportstatus",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalSchema: "fortreportstatus",
                        principalTable: "InboxState",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxState_OutboxId",
                        column: x => x.OutboxId,
                        principalSchema: "fortreportstatus",
                        principalTable: "OutboxState",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodLocationFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodLocationFloodReports",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodPropertyReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodPropertyReports",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_ConfirmedByUserId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodRoadFloodReports_FloodReportId",
                schema: "fortreportstatus",
                table: "FloodRoadFloodReports",
                column: "FloodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                schema: "fortreportstatus",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                schema: "fortreportstatus",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                schema: "fortreportstatus",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                schema: "fortreportstatus",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                schema: "fortreportstatus",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_Created",
                schema: "fortreportstatus",
                table: "OutboxState",
                column: "Created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodLocationFloodReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodPropertyReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodReportSources",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodRoadFloodReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodLocations",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodProperties",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodReports",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "FloodRoads",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "InboxState",
                schema: "fortreportstatus");

            migrationBuilder.DropTable(
                name: "OutboxState",
                schema: "fortreportstatus");
        }
    }
}
