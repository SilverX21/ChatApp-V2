using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedMessagesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasEdited",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserModelId",
                table: "Messages",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserModelId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "WasEdited",
                table: "Messages");
        }
    }
}
