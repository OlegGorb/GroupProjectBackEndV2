using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupProjectBackEndV2.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSpend_Users_UserId",
                table: "TimeSpend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSpend",
                table: "TimeSpend");

            migrationBuilder.RenameTable(
                name: "TimeSpend",
                newName: "Sessions");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSpend_UserId",
                table: "Sessions",
                newName: "IX_Sessions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_UserId",
                table: "Sessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_UserId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "TimeSpend");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_UserId",
                table: "TimeSpend",
                newName: "IX_TimeSpend_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSpend",
                table: "TimeSpend",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSpend_Users_UserId",
                table: "TimeSpend",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
