using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoSourcesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Articles_ArticleId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.RenameTable(
                name: "Links",
                newName: "InfoSources");

            migrationBuilder.RenameIndex(
                name: "IX_Links_UserId",
                table: "InfoSources",
                newName: "IX_InfoSources_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_ArticleId",
                table: "InfoSources",
                newName: "IX_InfoSources_ArticleId");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "InfoSources",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "InfoSources",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "InfoSources",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InfoSources",
                table: "InfoSources",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InfoSources_ApplicationUserId",
                table: "InfoSources",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoSources_Articles_ArticleId",
                table: "InfoSources",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoSources_AspNetUsers_ApplicationUserId",
                table: "InfoSources",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoSources_AspNetUsers_UserId",
                table: "InfoSources",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoSources_Articles_ArticleId",
                table: "InfoSources");

            migrationBuilder.DropForeignKey(
                name: "FK_InfoSources_AspNetUsers_ApplicationUserId",
                table: "InfoSources");

            migrationBuilder.DropForeignKey(
                name: "FK_InfoSources_AspNetUsers_UserId",
                table: "InfoSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InfoSources",
                table: "InfoSources");

            migrationBuilder.DropIndex(
                name: "IX_InfoSources_ApplicationUserId",
                table: "InfoSources");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "InfoSources");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "InfoSources");

            migrationBuilder.RenameTable(
                name: "InfoSources",
                newName: "Links");

            migrationBuilder.RenameIndex(
                name: "IX_InfoSources_UserId",
                table: "Links",
                newName: "IX_Links_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoSources_ArticleId",
                table: "Links",
                newName: "IX_Links_ArticleId");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Links",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Articles_ArticleId",
                table: "Links",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
