using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKnockoutMatchAndKnockoutBracket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnockoutBrackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnockoutBrackets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnockoutMatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BracketId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamPublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AwayTeamPublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeTeamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AwayTeamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HomePoints = table.Column<int>(type: "int", nullable: true),
                    AwayPoints = table.Column<int>(type: "int", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: false),
                    MatchOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WinnerPublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnockoutMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnockoutMatches_KnockoutBrackets_BracketId",
                        column: x => x.BracketId,
                        principalTable: "KnockoutBrackets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KnockoutBrackets_PublicId",
                table: "KnockoutBrackets",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KnockoutMatches_BracketId",
                table: "KnockoutMatches",
                column: "BracketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnockoutMatches");

            migrationBuilder.DropTable(
                name: "KnockoutBrackets");
        }
    }
}
