using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phaneritic.Implementations.Migrations.Operational
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "op");

            migrationBuilder.CreateSequence(
                name: "AccessMechanismID",
                schema: "op",
                startValue: 10000L,
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "AccessorID",
                schema: "op",
                startValue: 10000L,
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "AccessSessionID",
                schema: "op",
                startValue: 100000L,
                incrementBy: 100);

            migrationBuilder.CreateSequence(
                name: "OperationID",
                schema: "op",
                startValue: 100000L,
                incrementBy: 100);

            migrationBuilder.CreateSequence(
                name: "OperationLogID",
                schema: "op",
                startValue: 100000L,
                incrementBy: 100);

            migrationBuilder.CreateTable(
                name: "AccessGroups",
                schema: "op",
                columns: table => new
                {
                    AccessGroupKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroups", x => x.AccessGroupKey);
                });

            migrationBuilder.CreateTable(
                name: "AccessorCredentialTypes",
                schema: "op",
                columns: table => new
                {
                    AccessorCredentialTypeKey = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorCredentialTypes", x => x.AccessorCredentialTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "AccessorTypes",
                schema: "op",
                columns: table => new
                {
                    AccessorTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorTypes", x => x.AccessorTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "Methods",
                schema: "op",
                columns: table => new
                {
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    IsTransient = table.Column<bool>(type: "bit", nullable: false),
                    StayWithAccessMechanism = table.Column<bool>(type: "bit", nullable: false),
                    StayWithAccessor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.MethodKey);
                });

            migrationBuilder.CreateTable(
                name: "OperationLogs",
                schema: "op",
                columns: table => new
                {
                    OperationLogID = table.Column<long>(type: "bigint", nullable: false),
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    OperationID = table.Column<long>(type: "bigint", nullable: true),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    LogTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLogs", x => x.OperationLogID);
                });

            migrationBuilder.CreateTable(
                name: "OptionGroups",
                schema: "op",
                columns: table => new
                {
                    OptionGroupKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionGroups", x => x.OptionGroupKey);
                });

            migrationBuilder.CreateTable(
                name: "OptionTypes",
                schema: "op",
                columns: table => new
                {
                    OptionTypeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionTypes", x => x.OptionTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "ProcessNodeTypes",
                schema: "op",
                columns: table => new
                {
                    ProcessNodeTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessNodeTypes", x => x.ProcessNodeTypeKey);
                });

            migrationBuilder.CreateTable(
                name: "Accessors",
                schema: "op",
                columns: table => new
                {
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessorKey = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    AccessorTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessors", x => x.AccessorID);
                    table.ForeignKey(
                        name: "FK_Accessors_AccessorTypes_AccessorTypeKey",
                        column: x => x.AccessorTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessorTypes",
                        principalColumn: "AccessorTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessorTypeAccessorCredentialTypes",
                schema: "op",
                columns: table => new
                {
                    AccessorTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    AccessorCredentialTypeKey = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorTypeAccessorCredentialTypes", x => new { x.AccessorTypeKey, x.AccessorCredentialTypeKey });
                    table.ForeignKey(
                        name: "FK_AccessorTypeAccessorCredentialTypes_AccessorCredentialTypes_AccessorCredentialTypeKey",
                        column: x => x.AccessorCredentialTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessorCredentialTypes",
                        principalColumn: "AccessorCredentialTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessorTypeAccessorCredentialTypes_AccessorTypes_AccessorTypeKey",
                        column: x => x.AccessorTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessorTypes",
                        principalColumn: "AccessorTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessGroupMethods",
                schema: "op",
                columns: table => new
                {
                    AccessGroupKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroupMethods", x => new { x.AccessGroupKey, x.MethodKey });
                    table.ForeignKey(
                        name: "FK_AccessGroupMethods_AccessGroups_AccessGroupKey",
                        column: x => x.AccessGroupKey,
                        principalSchema: "op",
                        principalTable: "AccessGroups",
                        principalColumn: "AccessGroupKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessGroupMethods_Methods_MethodKey",
                        column: x => x.MethodKey,
                        principalSchema: "op",
                        principalTable: "Methods",
                        principalColumn: "MethodKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionGroupOptionTypes",
                schema: "op",
                columns: table => new
                {
                    OptionGroupKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    OptionTypeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionGroupOptionTypes", x => new { x.OptionGroupKey, x.OptionTypeKey });
                    table.ForeignKey(
                        name: "FK_OptionGroupOptionTypes_OptionGroups_OptionGroupKey",
                        column: x => x.OptionGroupKey,
                        principalSchema: "op",
                        principalTable: "OptionGroups",
                        principalColumn: "OptionGroupKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionGroupOptionTypes_OptionTypes_OptionTypeKey",
                        column: x => x.OptionTypeKey,
                        principalSchema: "op",
                        principalTable: "OptionTypes",
                        principalColumn: "OptionTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessMechanismTypes",
                schema: "op",
                columns: table => new
                {
                    AccessMechanismTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    IsUserAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsRoamingAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsPoolable = table.Column<bool>(type: "bit", nullable: false),
                    IsValidatedIPAddress = table.Column<bool>(type: "bit", nullable: false),
                    ProcessNodeTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessMechanismTypes", x => x.AccessMechanismTypeKey);
                    table.ForeignKey(
                        name: "FK_AccessMechanismTypes_ProcessNodeTypes_ProcessNodeTypeKey",
                        column: x => x.ProcessNodeTypeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodeTypes",
                        principalColumn: "ProcessNodeTypeKey");
                });

            migrationBuilder.CreateTable(
                name: "ProcessNodes",
                schema: "op",
                columns: table => new
                {
                    ProcessNodeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    ProcessNodeTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    ParentNodeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessNodes", x => x.ProcessNodeKey);
                    table.ForeignKey(
                        name: "FK_ProcessNodes_ProcessNodeTypes_ProcessNodeTypeKey",
                        column: x => x.ProcessNodeTypeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodeTypes",
                        principalColumn: "ProcessNodeTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessNodes_ProcessNodes_ParentNodeKey",
                        column: x => x.ParentNodeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodes",
                        principalColumn: "ProcessNodeKey");
                });

            migrationBuilder.CreateTable(
                name: "ProcessNodeTypeOptionGroups",
                schema: "op",
                columns: table => new
                {
                    ProcessNodeTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    OptionGroupKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessNodeTypeOptionGroups", x => new { x.ProcessNodeTypeKey, x.OptionGroupKey });
                    table.ForeignKey(
                        name: "FK_ProcessNodeTypeOptionGroups_OptionGroups_OptionGroupKey",
                        column: x => x.OptionGroupKey,
                        principalSchema: "op",
                        principalTable: "OptionGroups",
                        principalColumn: "OptionGroupKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessNodeTypeOptionGroups_ProcessNodeTypes_ProcessNodeTypeKey",
                        column: x => x.ProcessNodeTypeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodeTypes",
                        principalColumn: "ProcessNodeTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessorAccessGroups",
                schema: "op",
                columns: table => new
                {
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessGroupKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorAccessGroups", x => new { x.AccessorID, x.AccessGroupKey });
                    table.ForeignKey(
                        name: "FK_AccessorAccessGroups_AccessGroups_AccessGroupKey",
                        column: x => x.AccessGroupKey,
                        principalSchema: "op",
                        principalTable: "AccessGroups",
                        principalColumn: "AccessGroupKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessorAccessGroups_Accessors_AccessorID",
                        column: x => x.AccessorID,
                        principalSchema: "op",
                        principalTable: "Accessors",
                        principalColumn: "AccessorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessorCredentials",
                schema: "op",
                columns: table => new
                {
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessorCredentialTypeKey = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    CredentialValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorCredentials", x => new { x.AccessorID, x.AccessorCredentialTypeKey });
                    table.ForeignKey(
                        name: "FK_AccessorCredentials_AccessorCredentialTypes_AccessorCredentialTypeKey",
                        column: x => x.AccessorCredentialTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessorCredentialTypes",
                        principalColumn: "AccessorCredentialTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessorCredentials_Accessors_AccessorID",
                        column: x => x.AccessorID,
                        principalSchema: "op",
                        principalTable: "Accessors",
                        principalColumn: "AccessorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessorTypeAccessMechanismTypes",
                schema: "op",
                columns: table => new
                {
                    AccessorTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    AccessMechanismTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorTypeAccessMechanismTypes", x => new { x.AccessorTypeKey, x.AccessMechanismTypeKey });
                    table.ForeignKey(
                        name: "FK_AccessorTypeAccessMechanismTypes_AccessMechanismTypes_AccessMechanismTypeKey",
                        column: x => x.AccessMechanismTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessMechanismTypes",
                        principalColumn: "AccessMechanismTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessorTypeAccessMechanismTypes_AccessorTypes_AccessorTypeKey",
                        column: x => x.AccessorTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessorTypes",
                        principalColumn: "AccessorTypeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodAccessMechanismTypes",
                schema: "op",
                columns: table => new
                {
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    AccessMechanismTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodAccessMechanismTypes", x => new { x.MethodKey, x.AccessMechanismTypeKey });
                    table.ForeignKey(
                        name: "FK_MethodAccessMechanismTypes_AccessMechanismTypes_AccessMechanismTypeKey",
                        column: x => x.AccessMechanismTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessMechanismTypes",
                        principalColumn: "AccessMechanismTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodAccessMechanismTypes_Methods_MethodKey",
                        column: x => x.MethodKey,
                        principalSchema: "op",
                        principalTable: "Methods",
                        principalColumn: "MethodKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessMechanisms",
                schema: "op",
                columns: table => new
                {
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessMechanismKey = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    ProcessNodeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: true, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessMechanismTypeKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessMechanisms", x => x.AccessMechanismID);
                    table.ForeignKey(
                        name: "FK_AccessMechanisms_AccessMechanismTypes_AccessMechanismTypeKey",
                        column: x => x.AccessMechanismTypeKey,
                        principalSchema: "op",
                        principalTable: "AccessMechanismTypes",
                        principalColumn: "AccessMechanismTypeKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessMechanisms_ProcessNodes_ProcessNodeKey",
                        column: x => x.ProcessNodeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodes",
                        principalColumn: "ProcessNodeKey");
                });

            migrationBuilder.CreateTable(
                name: "Options",
                schema: "op",
                columns: table => new
                {
                    ProcessNodeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    OptionTypeKey = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    OptionValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => new { x.ProcessNodeKey, x.OptionTypeKey });
                    table.ForeignKey(
                        name: "FK_Options_ProcessNodes_ProcessNodeKey",
                        column: x => x.ProcessNodeKey,
                        principalSchema: "op",
                        principalTable: "ProcessNodes",
                        principalColumn: "ProcessNodeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessSessions",
                schema: "op",
                columns: table => new
                {
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AccessorID = table.Column<int>(type: "int", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessSessions", x => x.AccessSessionID);
                    table.ForeignKey(
                        name: "FK_AccessSessions_AccessMechanisms_AccessMechanismID",
                        column: x => x.AccessMechanismID,
                        principalSchema: "op",
                        principalTable: "AccessMechanisms",
                        principalColumn: "AccessMechanismID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessSessions_Accessors_AccessorID",
                        column: x => x.AccessorID,
                        principalSchema: "op",
                        principalTable: "Accessors",
                        principalColumn: "AccessorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "op",
                columns: table => new
                {
                    OperationID = table.Column<long>(type: "bigint", nullable: false),
                    AccessSessionID = table.Column<long>(type: "bigint", nullable: false),
                    MethodKey = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AccessMechanismID = table.Column<int>(type: "int", nullable: false),
                    AccessorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationID);
                    table.ForeignKey(
                        name: "FK_Operations_AccessMechanisms_AccessMechanismID",
                        column: x => x.AccessMechanismID,
                        principalSchema: "op",
                        principalTable: "AccessMechanisms",
                        principalColumn: "AccessMechanismID");
                    table.ForeignKey(
                        name: "FK_Operations_AccessSessions_AccessSessionID",
                        column: x => x.AccessSessionID,
                        principalSchema: "op",
                        principalTable: "AccessSessions",
                        principalColumn: "AccessSessionID");
                    table.ForeignKey(
                        name: "FK_Operations_Accessors_AccessorID",
                        column: x => x.AccessorID,
                        principalSchema: "op",
                        principalTable: "Accessors",
                        principalColumn: "AccessorID");
                    table.ForeignKey(
                        name: "FK_Operations_Methods_MethodKey",
                        column: x => x.MethodKey,
                        principalSchema: "op",
                        principalTable: "Methods",
                        principalColumn: "MethodKey");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupMethods_MethodKey",
                schema: "op",
                table: "AccessGroupMethods",
                column: "MethodKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroups_AccessGroupKey",
                schema: "op",
                table: "AccessGroups",
                column: "AccessGroupKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessMechanisms_AccessMechanismKey",
                schema: "op",
                table: "AccessMechanisms",
                column: "AccessMechanismKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessMechanisms_AccessMechanismTypeKey",
                schema: "op",
                table: "AccessMechanisms",
                column: "AccessMechanismTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessMechanisms_ProcessNodeKey",
                schema: "op",
                table: "AccessMechanisms",
                column: "ProcessNodeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessMechanismTypes_ProcessNodeTypeKey",
                schema: "op",
                table: "AccessMechanismTypes",
                column: "ProcessNodeTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessorAccessGroups_AccessGroupKey",
                schema: "op",
                table: "AccessorAccessGroups",
                column: "AccessGroupKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessorCredentials_AccessorCredentialTypeKey",
                schema: "op",
                table: "AccessorCredentials",
                column: "AccessorCredentialTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_Accessors_AccessorKey",
                schema: "op",
                table: "Accessors",
                column: "AccessorKey");

            migrationBuilder.CreateIndex(
                name: "IX_Accessors_AccessorTypeKey",
                schema: "op",
                table: "Accessors",
                column: "AccessorTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessorTypeAccessMechanismTypes_AccessMechanismTypeKey",
                schema: "op",
                table: "AccessorTypeAccessMechanismTypes",
                column: "AccessMechanismTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessorTypeAccessorCredentialTypes_AccessorCredentialTypeKey",
                schema: "op",
                table: "AccessorTypeAccessorCredentialTypes",
                column: "AccessorCredentialTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_AccessSessions_AccessMechanismID",
                schema: "op",
                table: "AccessSessions",
                column: "AccessMechanismID");

            migrationBuilder.CreateIndex(
                name: "IX_AccessSessions_AccessorID",
                schema: "op",
                table: "AccessSessions",
                column: "AccessorID");

            migrationBuilder.CreateIndex(
                name: "IX_MethodAccessMechanismTypes_AccessMechanismTypeKey",
                schema: "op",
                table: "MethodAccessMechanismTypes",
                column: "AccessMechanismTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccessMechanismID_AccessorID",
                schema: "op",
                table: "Operations",
                columns: new[] { "AccessMechanismID", "AccessorID" });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccessorID",
                schema: "op",
                table: "Operations",
                column: "AccessorID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccessSessionID",
                schema: "op",
                table: "Operations",
                column: "AccessSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_MethodKey",
                schema: "op",
                table: "Operations",
                column: "MethodKey");

            migrationBuilder.CreateIndex(
                name: "IX_OptionGroupOptionTypes_OptionTypeKey",
                schema: "op",
                table: "OptionGroupOptionTypes",
                column: "OptionTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNodes_ParentNodeKey",
                schema: "op",
                table: "ProcessNodes",
                column: "ParentNodeKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNodes_ProcessNodeTypeKey",
                schema: "op",
                table: "ProcessNodes",
                column: "ProcessNodeTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNodeTypeOptionGroups_OptionGroupKey",
                schema: "op",
                table: "ProcessNodeTypeOptionGroups",
                column: "OptionGroupKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessGroupMethods",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorAccessGroups",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorCredentials",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorTypeAccessMechanismTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorTypeAccessorCredentialTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "MethodAccessMechanismTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "OperationLogs",
                schema: "op");

            migrationBuilder.DropTable(
                name: "Operations",
                schema: "op");

            migrationBuilder.DropTable(
                name: "OptionGroupOptionTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "Options",
                schema: "op");

            migrationBuilder.DropTable(
                name: "ProcessNodeTypeOptionGroups",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessGroups",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorCredentialTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessSessions",
                schema: "op");

            migrationBuilder.DropTable(
                name: "Methods",
                schema: "op");

            migrationBuilder.DropTable(
                name: "OptionTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "OptionGroups",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessMechanisms",
                schema: "op");

            migrationBuilder.DropTable(
                name: "Accessors",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessMechanismTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "ProcessNodes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "AccessorTypes",
                schema: "op");

            migrationBuilder.DropTable(
                name: "ProcessNodeTypes",
                schema: "op");

            migrationBuilder.DropSequence(
                name: "AccessMechanismID",
                schema: "op");

            migrationBuilder.DropSequence(
                name: "AccessorID",
                schema: "op");

            migrationBuilder.DropSequence(
                name: "AccessSessionID",
                schema: "op");

            migrationBuilder.DropSequence(
                name: "OperationID",
                schema: "op");

            migrationBuilder.DropSequence(
                name: "OperationLogID",
                schema: "op");
        }
    }
}
