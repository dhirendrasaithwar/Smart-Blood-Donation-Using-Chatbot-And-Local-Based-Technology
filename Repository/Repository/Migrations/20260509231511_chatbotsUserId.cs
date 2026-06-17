using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class chatbotsUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ChatBots",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "ChatBots",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatBots_UserId",
                table: "ChatBots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBots_UserId1",
                table: "ChatBots",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBots_User_UserId",
                table: "ChatBots",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBots_User_UserId1",
                table: "ChatBots",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBots_User_UserId",
                table: "ChatBots");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatBots_User_UserId1",
                table: "ChatBots");

            migrationBuilder.DropIndex(
                name: "IX_ChatBots_UserId",
                table: "ChatBots");

            migrationBuilder.DropIndex(
                name: "IX_ChatBots_UserId1",
                table: "ChatBots");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatBots");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ChatBots");
        }
    }
}
