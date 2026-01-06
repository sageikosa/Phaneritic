using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phaneritic.Implementations.Migrations.Ledgering
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ldgr");

            migrationBuilder.CreateSequence(
                name: "ActivityID",
                schema: "ldgr",
                startValue: 10000L,
                incrementBy: 100);

            migrationBuilder.CreateTable(
                name: "ActivityTypes",
                schema: "ldgr",
                columns: table => new
                {
                    ActivityTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Category = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypes", x => x.ActivityTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "ldgr",
                columns: table => new
                {
                    ActivityID = table.Column<long>(type: "bigint", nullable: false),
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    ActivityTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    OperationID = table.Column<long>(type: "bigint", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DurationMicroSeconds = table.Column<long>(type: "bigint", nullable: false),
                    DurationMilliSeconds = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    EntryCount = table.Column<int>(type: "int", nullable: false),
                    InfoEntryCount = table.Column<int>(type: "int", nullable: false),
                    ExceptionEntryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTypes_ActivityTypeKey",
                        column: x => x.ActivityTypeKey,
                        principalSchema: "ldgr",
                        principalTable: "ActivityTypes",
                        principalColumn: "ActivityTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionEntries",
                schema: "ldgr",
                columns: table => new
                {
                    ActivityID = table.Column<long>(type: "bigint", nullable: false),
                    EntryIndex = table.Column<int>(type: "int", nullable: false),
                    ExceptionName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    ActivityTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    OperationID = table.Column<long>(type: "bigint", nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OffsetMicroSeconds = table.Column<long>(type: "bigint", nullable: false),
                    OffsetMilliSeconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionEntries", x => new { x.ActivityID, x.EntryIndex });
                    table.ForeignKey(
                        name: "FK_ExceptionEntries_Activities_ActivityID",
                        column: x => x.ActivityID,
                        principalSchema: "ldgr",
                        principalTable: "Activities",
                        principalColumn: "ActivityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoEntries",
                schema: "ldgr",
                columns: table => new
                {
                    ActivityID = table.Column<long>(type: "bigint", nullable: false),
                    EntryIndex = table.Column<int>(type: "int", nullable: false),
                    InfoEntryKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    InfoEntryValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    ActivityTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    OperationID = table.Column<long>(type: "bigint", nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OffsetMicroSeconds = table.Column<long>(type: "bigint", nullable: false),
                    OffsetMilliSeconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoEntries", x => new { x.ActivityID, x.EntryIndex, x.InfoEntryKey });
                    table.ForeignKey(
                        name: "FK_InfoEntries_Activities_ActivityID",
                        column: x => x.ActivityID,
                        principalSchema: "ldgr",
                        principalTable: "Activities",
                        principalColumn: "ActivityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTypeKey",
                schema: "ldgr",
                table: "Activities",
                column: "ActivityTypeKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionEntries",
                schema: "ldgr");

            migrationBuilder.DropTable(
                name: "InfoEntries",
                schema: "ldgr");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "ldgr");

            migrationBuilder.DropTable(
                name: "ActivityTypes",
                schema: "ldgr");

            migrationBuilder.DropSequence(
                name: "ActivityID",
                schema: "ldgr");
        }
    }
}
