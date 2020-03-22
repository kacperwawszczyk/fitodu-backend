using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ModifiedtheTraingResultTableKeyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults");

            migrationBuilder.DropColumn(
                name: "IdTrainingExercise",
                table: "TrainingResults");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
