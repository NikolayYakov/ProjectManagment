using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagment.Migrations
{
    /// <inheritdoc />
    public partial class dbWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Issues",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ProjectsToMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "LabelsToIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEpic",
                table: "Issues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApplicationUserIssue",
                columns: table => new
                {
                    AssigneesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssuesAssignedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserIssue", x => new { x.AssigneesId, x.IssuesAssignedId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserIssue_AspNetUsers_AssigneesId",
                        column: x => x.AssigneesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIssue_Issues_IssuesAssignedId",
                        column: x => x.IssuesAssignedId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersToIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToIssues_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersToIssues_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIssue_IssuesAssignedId",
                table: "ApplicationUserIssue",
                column: "IssuesAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToIssues_IssueId",
                table: "UsersToIssues",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToIssues_UserId",
                table: "UsersToIssues",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIssue");

            migrationBuilder.DropTable(
                name: "UsersToIssues");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ProjectsToMembers");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "LabelsToIssues");

            migrationBuilder.DropColumn(
                name: "IsEpic",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Issues",
                newName: "isDeleted");
        }
    }
}
