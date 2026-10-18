using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Razor.Migrations
{
    /// <inheritdoc />
    public partial class Taxons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Authors_AuthorID",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Posts",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Posts",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "Posts",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorID",
                table: "Posts",
                newName: "IX_Posts_AuthorId");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Authors",
                newName: "AuthorId");

            migrationBuilder.CreateTable(
                name: "Taxons",
                columns: table => new
                {
                    TaxonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VernacularName = table.Column<string>(type: "TEXT", nullable: false),
                    dwc_TaxonID = table.Column<string>(type: "TEXT", nullable: false),
                    ParentTaxonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxons", x => x.TaxonId);
                    table.ForeignKey(
                        name: "FK_Taxons_Taxons_ParentTaxonId",
                        column: x => x.ParentTaxonId,
                        principalTable: "Taxons",
                        principalColumn: "TaxonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taxons_ParentTaxonId",
                table: "Taxons",
                column: "ParentTaxonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Authors_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Authors_AuthorId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Taxons");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Posts",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Posts",
                newName: "AuthorID");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "PostID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                newName: "IX_Posts_AuthorID");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Authors",
                newName: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Authors_AuthorID",
                table: "Posts",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
