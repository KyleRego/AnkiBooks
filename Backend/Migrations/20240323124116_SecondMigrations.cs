using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Public = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentArticleId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Articles_ParentArticleId",
                        column: x => x.ParentArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleElements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ArticleId = table.Column<string>(type: "TEXT", nullable: false),
                    OrdinalPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    ConceptId = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Front = table.Column<string>(type: "TEXT", nullable: true),
                    Back = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleElements_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Concepts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Public = table.Column<bool>(type: "INTEGER", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false),
                    BasicNoteId = table.Column<string>(type: "TEXT", nullable: true),
                    ClozeNoteId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concepts_ArticleElements_BasicNoteId",
                        column: x => x.BasicNoteId,
                        principalTable: "ArticleElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Concepts_ArticleElements_ClozeNoteId",
                        column: x => x.ClozeNoteId,
                        principalTable: "ArticleElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Concepts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptName",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ConceptId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptName_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleElements_ArticleId_OrdinalPosition",
                table: "ArticleElements",
                columns: new[] { "ArticleId", "OrdinalPosition" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleElements_ConceptId",
                table: "ArticleElements",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ParentArticleId",
                table: "Articles",
                column: "ParentArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptName_ConceptId",
                table: "ConceptName",
                column: "ConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_ApplicationUserId",
                table: "Concepts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_BasicNoteId",
                table: "Concepts",
                column: "BasicNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_ClozeNoteId",
                table: "Concepts",
                column: "ClozeNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleElements_Concepts_ConceptId",
                table: "ArticleElements",
                column: "ConceptId",
                principalTable: "Concepts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleElements_Articles_ArticleId",
                table: "ArticleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleElements_Concepts_ConceptId",
                table: "ArticleElements");

            migrationBuilder.DropTable(
                name: "ConceptName");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Concepts");

            migrationBuilder.DropTable(
                name: "ArticleElements");
        }
    }
}
