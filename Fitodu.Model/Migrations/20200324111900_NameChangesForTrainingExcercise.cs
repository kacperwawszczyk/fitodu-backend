using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class NameChangesForTrainingExcercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerNote",
                table: "TrainingExercises",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "IdTrainingExercise",
                table: "TrainingExercises",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "TrainingExercises",
                newName: "TrainerNote");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TrainingExercises",
                newName: "IdTrainingExercise");
        }
    }
}
