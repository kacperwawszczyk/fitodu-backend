using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ContextAndConfigurationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Maximums",
                table: "Maximums");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Maximums");

            migrationBuilder.RenameColumn(
                name: "Repeats",
                table: "TrainingResults",
                newName: "Repetitions");

            migrationBuilder.RenameColumn(
                name: "Repeats",
                table: "TrainingExercises",
                newName: "Repetitions");

            migrationBuilder.AlterColumn<string>(
                name: "IdExercise",
                table: "Maximums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "Maximums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maximums",
                table: "Maximums",
                columns: new[] { "IdExercise", "IdClient" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Maximums",
                table: "Maximums");

            migrationBuilder.RenameColumn(
                name: "Repetitions",
                table: "TrainingResults",
                newName: "Repeats");

            migrationBuilder.RenameColumn(
                name: "Repetitions",
                table: "TrainingExercises",
                newName: "Repeats");

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "Maximums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdExercise",
                table: "Maximums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Maximums",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maximums",
                table: "Maximums",
                column: "Id");
        }
    }
}
