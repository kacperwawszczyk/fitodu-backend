using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class TrainingExerciseFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TrainingExercises");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TrainingExercises");

            migrationBuilder.AddColumn<int>(
                name: "RepetitionsResult",
                table: "TrainingExercises",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeResult",
                table: "TrainingExercises",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepetitionsResult",
                table: "TrainingExercises");

            migrationBuilder.DropColumn(
                name: "TimeResult",
                table: "TrainingExercises");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TrainingExercises",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TrainingExercises",
                type: "text",
                nullable: true);
        }
    }
}
