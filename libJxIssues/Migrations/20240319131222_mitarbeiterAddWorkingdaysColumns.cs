using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace libJxIssues.Migrations
{
    /// <inheritdoc />
    public partial class mitarbeiterAddWorkingdaysColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "gitToken",
                table: "mitarbeiter",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "dienstag",
                table: "mitarbeiter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "donnerstag",
                table: "mitarbeiter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "freitag",
                table: "mitarbeiter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "mittwoch",
                table: "mitarbeiter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "montag",
                table: "mitarbeiter",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dienstag",
                table: "mitarbeiter");

            migrationBuilder.DropColumn(
                name: "donnerstag",
                table: "mitarbeiter");

            migrationBuilder.DropColumn(
                name: "freitag",
                table: "mitarbeiter");

            migrationBuilder.DropColumn(
                name: "mittwoch",
                table: "mitarbeiter");

            migrationBuilder.DropColumn(
                name: "montag",
                table: "mitarbeiter");

            migrationBuilder.AlterColumn<string>(
                name: "gitToken",
                table: "mitarbeiter",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }
    }
}
