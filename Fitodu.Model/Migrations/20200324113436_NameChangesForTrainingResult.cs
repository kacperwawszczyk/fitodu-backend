using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class NameChangesForTrainingResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_TrainingResults_IdTrainingResult",
                table: "TrainingResults");

            migrationBuilder.RenameColumn(
                name: "IdTrainingResult",
                table: "TrainingResults",
                newName: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TrainingResults_Id",
                table: "TrainingResults",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_TrainingResults_Id",
                table: "TrainingResults");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TrainingResults",
                newName: "IdTrainingResult");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TrainingResults_IdTrainingResult",
                table: "TrainingResults",
                column: "IdTrainingResult");
        }
    }
}
