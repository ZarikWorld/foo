using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class jxIssueNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "aktenzahl",
                table: "jxIssue",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "kunde",
                table: "jxIssue",
                type: "varchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aktenzahl",
                table: "jxIssue");

            migrationBuilder.DropColumn(
                name: "kunde",
                table: "jxIssue");
        }
    }
}
