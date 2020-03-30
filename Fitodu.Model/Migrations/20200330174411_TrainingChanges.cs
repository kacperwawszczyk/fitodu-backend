using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class TrainingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trainings");
        }
    }
}
