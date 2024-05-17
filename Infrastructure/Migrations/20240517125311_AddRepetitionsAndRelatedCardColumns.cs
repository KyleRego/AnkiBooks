using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRepetitionsAndRelatedCardColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClozeNotes_ArticleElements_DeckId",
                table: "ClozeNotes");

            migrationBuilder.DropTable(
                name: "BasicNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClozeNotes",
                table: "ClozeNotes");

            migrationBuilder.RenameTable(
                name: "ClozeNotes",
                newName: "Card");

            migrationBuilder.RenameIndex(
                name: "IX_ClozeNotes_DeckId",
                table: "Card",
                newName: "IX_Card_DeckId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Card",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Back",
                table: "Card",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Card",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "EasinessFactor",
                table: "Card",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Front",
                table: "Card",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterRepetitionInterval",
                table: "Card",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastReviewedAt",
                table: "Card",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulRecallStreak",
                table: "Card",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Card",
                table: "Card",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Repetitions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CardId = table.Column<string>(type: "TEXT", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repetitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repetitions_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repetitions_CardId",
                table: "Repetitions",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_ArticleElements_DeckId",
                table: "Card",
                column: "DeckId",
                principalTable: "ArticleElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_ArticleElements_DeckId",
                table: "Card");

            migrationBuilder.DropTable(
                name: "Repetitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Card",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "Back",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "EasinessFactor",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "Front",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "InterRepetitionInterval",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "LastReviewedAt",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "SuccessfulRecallStreak",
                table: "Card");

            migrationBuilder.RenameTable(
                name: "Card",
                newName: "ClozeNotes");

            migrationBuilder.RenameIndex(
                name: "IX_Card_DeckId",
                table: "ClozeNotes",
                newName: "IX_ClozeNotes_DeckId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "ClozeNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClozeNotes",
                table: "ClozeNotes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BasicNotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DeckId = table.Column<string>(type: "TEXT", nullable: false),
                    Back = table.Column<string>(type: "TEXT", nullable: false),
                    Front = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_BasicNotes_DeckId",
                table: "BasicNotes",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClozeNotes_ArticleElements_DeckId",
                table: "ClozeNotes",
                column: "DeckId",
                principalTable: "ArticleElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
