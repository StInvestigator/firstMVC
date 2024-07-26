using firstMVC.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firstMVC.Migrations
{
    /// <inheritdoc />
    public partial class ReviewCurrentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: (int)CurrentStatus.Draft);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reviews");
        }
    }
}
