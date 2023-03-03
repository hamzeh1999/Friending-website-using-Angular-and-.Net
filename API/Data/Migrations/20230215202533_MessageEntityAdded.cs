using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class MessageEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    senderId = table.Column<int>(type: "INTEGER", nullable: false),
                    senderUserName = table.Column<string>(type: "TEXT", nullable: true),
                    recipientId = table.Column<int>(type: "INTEGER", nullable: false),
                    recipientUserName = table.Column<string>(type: "TEXT", nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    dateRead = table.Column<DateTime>(type: "TEXT", nullable: true),
                    messageSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    senderDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    recipientDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.id);
                    table.ForeignKey(
                        name: "FK_Message_AppUser_recipientId",
                        column: x => x.recipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_AppUser_senderId",
                        column: x => x.senderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_recipientId",
                table: "Message",
                column: "recipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_senderId",
                table: "Message",
                column: "senderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
