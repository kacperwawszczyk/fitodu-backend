using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scheduledo.Model.Migrations
{
    public partial class ModifiedtheTrainingExerciseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingExercises",
                table: "TrainingExercises");

            migrationBuilder.AddColumn<int>(
                name: "IdTrainingExercise",
                table: "TrainingExercises",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingExercises",
                table: "TrainingExercises",
                column: "IdTrainingExercise");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingExercises",
                table: "TrainingExercises");

            migrationBuilder.DropColumn(
                name: "IdTrainingExercise",
                table: "TrainingExercises");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingExercises",
                table: "TrainingExercises",
                columns: new[] { "IdTraining", "IdExercise" });
        }
    }
}
