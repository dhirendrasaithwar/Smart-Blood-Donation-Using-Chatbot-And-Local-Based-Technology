using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class donars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DonarRequests",
                columns: table => new
                {
                    DonarRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonarId = table.Column<long>(type: "bigint", nullable: false),
                    RequesterId = table.Column<long>(type: "bigint", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonarRequests", x => x.DonarRequestId);
                    table.ForeignKey(
                        name: "FK_DonarRequests_User_DonarId",
                        column: x => x.DonarId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonarRequests_User_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonarRequests_DonarId",
                table: "DonarRequests",
                column: "DonarId");

            migrationBuilder.CreateIndex(
                name: "IX_DonarRequests_RequesterId",
                table: "DonarRequests",
                column: "RequesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonarRequests");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "User");
        }
    }
}
