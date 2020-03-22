using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ModifiedtheTraingResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults");

            migrationBuilder.AddColumn<int>(
                name: "IdTrainingExercise",
                table: "TrainingResults",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults",
                column: "IdTrainingExercise");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults");

            migrationBuilder.DropColumn(
                name: "IdTrainingExercise",
                table: "TrainingResults");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults",
                columns: new[] { "IdExercise", "IdTraining" });
        }
    }
}
