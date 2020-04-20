using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class addedTitleToAwaitingTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AwaitingTrainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AwaitingTrainings");
        }
    }
}
