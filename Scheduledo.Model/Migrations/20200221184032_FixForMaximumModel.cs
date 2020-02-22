using Microsoft.EntityFrameworkCore.Migrations;

namespace Scheduledo.Model.Migrations
{
    public partial class FixForMaximumModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdExercise",
                table: "Maximums",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdExercise",
                table: "Maximums",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
