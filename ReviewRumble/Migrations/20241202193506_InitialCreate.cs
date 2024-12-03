using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewRumble.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviewers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrsReviewed = table.Column<int>(type: "int", nullable: false),
                    PendingPrsToReview = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PullRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Repository = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstReviewerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondReviewerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedFirstReviewerId = table.Column<int>(type: "int", nullable: false),
                    AssignedSecondReviewerId = table.Column<int>(type: "int", nullable: false),
                    ReviewerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequests_Reviewers_AssignedFirstReviewerId",
                        column: x => x.AssignedFirstReviewerId,
                        principalTable: "Reviewers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequests_Reviewers_AssignedSecondReviewerId",
                        column: x => x.AssignedSecondReviewerId,
                        principalTable: "Reviewers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequests_Reviewers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_AssignedFirstReviewerId",
                table: "PullRequests",
                column: "AssignedFirstReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_AssignedSecondReviewerId",
                table: "PullRequests",
                column: "AssignedSecondReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_ReviewerId",
                table: "PullRequests",
                column: "ReviewerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PullRequests");

            migrationBuilder.DropTable(
                name: "Reviewers");
        }
    }
}
