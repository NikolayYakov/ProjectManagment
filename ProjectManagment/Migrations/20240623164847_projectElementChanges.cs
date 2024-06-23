using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagment.Migrations
{
    /// <inheritdoc />
    public partial class projectElementChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Milestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Milestones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Labels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Areas");
        }
    }
}
