using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDecksAsArticleElementsWithManyBasicNotesAndClozeNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Back",
                table: "ArticleElements");

            migrationBuilder.DropColumn(
                name: "Front",
                table: "ArticleElements");

            migrationBuilder.RenameColumn(
                name: "MarkdownContent_Text",
                table: "ArticleElements",
                newName: "Description");

            migrationBuilder.CreateTable(
                name: "BasicNotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Front = table.Column<string>(type: "TEXT", nullable: false),
                    Back = table.Column<string>(type: "TEXT", nullable: false),
                    DeckId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicNotes_ArticleElements_DeckId",
                        column: x => x.DeckId,
                        principalTable: "ArticleElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClozeNotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    DeckId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClozeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClozeNotes_ArticleElements_DeckId",
                        column: x => x.DeckId,
                        principalTable: "ArticleElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicNotes_DeckId",
                table: "BasicNotes",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_ClozeNotes_DeckId",
                table: "ClozeNotes",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicNotes");

            migrationBuilder.DropTable(
                name: "ClozeNotes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ArticleElements",
                newName: "MarkdownContent_Text");

            migrationBuilder.AddColumn<string>(
                name: "Back",
                table: "ArticleElements",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Front",
                table: "ArticleElements",
                type: "TEXT",
                nullable: true);
        }
    }
}
