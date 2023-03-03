using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class likeEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    sourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    likedUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => new { x.sourceUserId, x.likedUserId });
                    table.ForeignKey(
                        name: "FK_likes_AppUser_likedUserId",
                        column: x => x.likedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_AppUser_sourceUserId",
                        column: x => x.sourceUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_likes_likedUserId",
                table: "likes",
                column: "likedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateOfBirth",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
