using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firstMVC.Migrations
{
    /// <inheritdoc />
    public partial class UserCreatedByCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorId",
                table: "Users",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_CreatorId",
                table: "Users",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_CreatorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Users");
        }
    }
}
