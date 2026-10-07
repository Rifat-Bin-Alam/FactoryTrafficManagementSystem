using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryTrafficManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JunctionId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EventType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Direction = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    PreviousState = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    NewState = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CommandId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControllerCommands",
                columns: table => new
                {
                    CommandId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    JunctionId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedState = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerCommands", x => x.CommandId);
                });

            migrationBuilder.CreateTable(
                name: "Junctions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentPhase = table.Column<int>(type: "INTEGER", nullable: false),
                    DesiredNorth = table.Column<int>(type: "INTEGER", nullable: false),
                    DesiredSouth = table.Column<int>(type: "INTEGER", nullable: false),
                    DesiredEast = table.Column<int>(type: "INTEGER", nullable: false),
                    DesiredWest = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualNorth = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualSouth = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualEast = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualWest = table.Column<int>(type: "INTEGER", nullable: false),
                    ControllerOnline = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhaseStartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Junctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorEvents",
                columns: table => new
                {
                    EventId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    JunctionId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    SequenceNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    SensorTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorEvents", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    JunctionId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ArrivedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsQueued = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_JunctionId_Timestamp",
                table: "AuditLogs",
                columns: new[] { "JunctionId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ControllerCommands_CommandId",
                table: "ControllerCommands",
                column: "CommandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControllerCommands_JunctionId_Status",
                table: "ControllerCommands",
                columns: new[] { "JunctionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SensorEvents_EventId",
                table: "SensorEvents",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorEvents_JunctionId_SequenceNumber",
                table: "SensorEvents",
                columns: new[] { "JunctionId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_JunctionId_IsQueued",
                table: "Vehicles",
                columns: new[] { "JunctionId", "IsQueued" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ControllerCommands");

            migrationBuilder.DropTable(
                name: "Junctions");

            migrationBuilder.DropTable(
                name: "SensorEvents");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
