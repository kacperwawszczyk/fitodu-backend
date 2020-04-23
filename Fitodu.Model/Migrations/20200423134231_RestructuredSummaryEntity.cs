using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class RestructuredSummaryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Trainings_IdTraining",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_IdTraining",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "IdTraining",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "WeightChange",
                table: "Summaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Summaries",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FatPercentage",
                table: "Summaries",
                type: "decimal(4, 2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdClient",
                table: "Summaries",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Summaries",
                type: "decimal(5, 2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_IdClient",
                table: "Summaries",
                column: "IdClient");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Clients_IdClient",
                table: "Summaries",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Clients_IdClient",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_IdClient",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "FatPercentage",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "IdClient",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Summaries");

            migrationBuilder.AddColumn<int>(
                name: "IdTraining",
                table: "Summaries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightChange",
                table: "Summaries",
                type: "decimal(3, 2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_IdTraining",
                table: "Summaries",
                column: "IdTraining");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Trainings_IdTraining",
                table: "Summaries",
                column: "IdTraining",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
