using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phaneritic.Implementations.EF.Migrations.Kernel
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "krnl");

            migrationBuilder.CreateTable(
                name: "TableFreshness",
                schema: "krnl",
                columns: table => new
                {
                    TableKey = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ConcurrencyCheck = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableFreshness", x => x.TableKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableFreshness",
                schema: "krnl");
        }
    }
}
