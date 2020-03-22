using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ChangesRequiredByCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "AwaitingTrainings");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Trainings",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Trainings",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AwaitingTrainings",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AwaitingTrainings",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "WeekPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCoach = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Day = table.Column<int>(nullable: false),
                    WeekPlanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayPlans_WeekPlans_WeekPlanId",
                        column: x => x.WeekPlanId,
                        principalTable: "WeekPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DayPlanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTimes_DayPlans_DayPlanId",
                        column: x => x.DayPlanId,
                        principalTable: "DayPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayPlans_WeekPlanId",
                table: "DayPlans",
                column: "WeekPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTimes_DayPlanId",
                table: "WorkoutTimes",
                column: "DayPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutTimes");

            migrationBuilder.DropTable(
                name: "DayPlans");

            migrationBuilder.DropTable(
                name: "WeekPlans");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AwaitingTrainings");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AwaitingTrainings");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Trainings",
                newName: "Date");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "AwaitingTrainings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
