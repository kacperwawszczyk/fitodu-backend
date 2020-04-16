using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class configtest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
