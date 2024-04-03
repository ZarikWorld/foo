using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class updatelastSyncInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lastSyncInfo");

            migrationBuilder.CreateTable(
                name: "programInformation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lastSyncGIT = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programInformation", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "programInformation");

            migrationBuilder.CreateTable(
                name: "lastSyncInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lastSync = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lastSyncInfo", x => x.id);
                });
        }
    }
}
