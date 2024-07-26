using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firstMVC.Migrations
{
    /// <inheritdoc />
    public partial class ModeratorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Name", "NormalizedName" },
               values: new object[,]
               {
                    { "Moderator", "MODERATOR" }
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
