using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jxIssue",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    git = table.Column<bool>(type: "bit", nullable: false),
                    originallink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sortorder = table.Column<int>(type: "int", nullable: true),
                    start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mitarbeiter_id = table.Column<int>(type: "int", nullable: true),
                    schaetzung = table.Column<int>(type: "int", nullable: true),
                    deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    typ = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jxIssue", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mitarbeiter",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gitUsername = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mitarbeiter", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jxIssue");

            migrationBuilder.DropTable(
                name: "mitarbeiter");
        }
    }
}
