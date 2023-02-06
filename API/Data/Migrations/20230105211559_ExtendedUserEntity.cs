using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            
         
            
         

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "AppUser",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateOfBirth",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "interests",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "introduction",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "knownAs",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastActivity",
                table: "AppUser",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "lookingFor",
                table: "AppUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    isMan = table.Column<int>(type: "INTEGER", nullable: false),
                    publicId = table.Column<string>(type: "TEXT", nullable: true),
                    appUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_Photo_AppUser_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photo_appUserId",
                table: "Photo",
                column: "appUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropColumn(
                name: "city",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "country",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "created",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "dateOfBirth",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "interests",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "introduction",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "knownAs",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "lastActivity",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "lookingFor",
                table: "AppUser");
        }
    }
}
