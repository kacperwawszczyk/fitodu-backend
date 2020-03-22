using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ModifiedtheTraingResultTableKeyChanges5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTrainingExercise",
                table: "TrainingResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingResults_IdTrainingExercise",
                table: "TrainingResults",
                column: "IdTrainingExercise",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingResults_IdTrainingExercise",
                table: "TrainingResults");

            migrationBuilder.DropColumn(
                name: "IdTrainingExercise",
                table: "TrainingResults");
        }
    }
}
