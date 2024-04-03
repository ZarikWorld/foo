using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class jxIssueIID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "erledigt",
                table: "jxIssue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "iid",
                table: "jxIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "prioPunkte",
                table: "jxIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "erledigt",
                table: "jxIssue");

            migrationBuilder.DropColumn(
                name: "iid",
                table: "jxIssue");

            migrationBuilder.DropColumn(
                name: "prioPunkte",
                table: "jxIssue");
        }
    }
}
