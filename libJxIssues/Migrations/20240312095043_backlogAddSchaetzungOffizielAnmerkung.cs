using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class backlogAddSchaetzungOffizielAnmerkung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "anmerkung",
                table: "jxIssue",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "schaetzungOffiziell",
                table: "jxIssue",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "anmerkung",
                table: "jxIssue");

            migrationBuilder.DropColumn(
                name: "schaetzungOffiziell",
                table: "jxIssue");
        }
    }
}
