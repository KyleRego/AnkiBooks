using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRepetitionOccuredAtToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastReviewedAt",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SuccessfulRecallStreak",
                table: "Cards");

            migrationBuilder.AlterColumn<long>(
                name: "OccurredAt",
                table: "Repetitions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OccurredAt",
                table: "Repetitions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastReviewedAt",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulRecallStreak",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
