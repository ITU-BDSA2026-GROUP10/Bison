using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FavoritePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoritePosts",
                table: "Authors",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoritePosts",
                table: "Authors");
        }
    }
}
