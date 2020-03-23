using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class CancelTimeAndAwaitingTrainingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToResign",
                table: "Coaches");

            migrationBuilder.AddColumn<int>(
                name: "CancelTimeHours",
                table: "Coaches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CancelTimeMinutes",
                table: "Coaches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Receiver",
                table: "AwaitingTrainings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sender",
                table: "AwaitingTrainings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelTimeHours",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CancelTimeMinutes",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "AwaitingTrainings");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "AwaitingTrainings");

            migrationBuilder.AddColumn<string>(
                name: "TimeToResign",
                table: "Coaches",
                nullable: true);
        }
    }
}
