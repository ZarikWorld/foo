using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class updateJxIssuesPrioPunkte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "prioPunkte",
                table: "jxIssue",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "prioPunkte",
                table: "jxIssue",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
