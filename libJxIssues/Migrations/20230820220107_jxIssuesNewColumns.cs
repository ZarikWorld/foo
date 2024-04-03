using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class jxIssuesNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "originallink",
                table: "jxIssue");

            migrationBuilder.RenameColumn(
                name: "sortorder",
                table: "jxIssue",
                newName: "sortOrder");

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "jxIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "web_url",
                table: "jxIssue",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "project_id",
                table: "jxIssue");

            migrationBuilder.DropColumn(
                name: "web_url",
                table: "jxIssue");

            migrationBuilder.RenameColumn(
                name: "sortOrder",
                table: "jxIssue",
                newName: "sortorder");

            migrationBuilder.AddColumn<string>(
                name: "originallink",
                table: "jxIssue",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
