using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class jxIssueNewColumnCreatorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "creator_id",
                table: "jxIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "jxIssue");
        }
    }
}
