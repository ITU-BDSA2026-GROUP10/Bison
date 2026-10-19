using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Razor.Migrations
{
    /// <inheritdoc />
    public partial class commentsAndProposals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Posts",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ObservationPostId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Proposal_ObservationPostId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Proposal_TaxonId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxonId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ObservationPostId",
                table: "Posts",
                column: "ObservationPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Proposal_ObservationPostId",
                table: "Posts",
                column: "Proposal_ObservationPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Proposal_TaxonId",
                table: "Posts",
                column: "Proposal_TaxonId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TaxonId",
                table: "Posts",
                column: "TaxonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_ObservationPostId",
                table: "Posts",
                column: "ObservationPostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_Proposal_ObservationPostId",
                table: "Posts",
                column: "Proposal_ObservationPostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Taxons_Proposal_TaxonId",
                table: "Posts",
                column: "Proposal_TaxonId",
                principalTable: "Taxons",
                principalColumn: "TaxonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts",
                column: "TaxonId",
                principalTable: "Taxons",
                principalColumn: "TaxonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_ObservationPostId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_Proposal_ObservationPostId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Taxons_Proposal_TaxonId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ObservationPostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Proposal_ObservationPostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Proposal_TaxonId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TaxonId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ObservationPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Proposal_ObservationPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Proposal_TaxonId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TaxonId",
                table: "Posts");
        }
    }
}
