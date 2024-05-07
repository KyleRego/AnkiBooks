using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleIdToLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleId",
                table: "Links",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_ArticleId",
                table: "Links",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Articles_ArticleId",
                table: "Links",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Articles_ArticleId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_ArticleId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Links");
        }
    }
}
