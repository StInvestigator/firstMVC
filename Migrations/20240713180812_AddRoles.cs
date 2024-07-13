using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firstMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"AspNetRoles",
                columns: new[] {"Name","NormalizedName"},
                values: new object[,]
                {
                    { "Admin", "ADMIN" },
                    { "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // nothing here
        }
    }
}
