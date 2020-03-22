using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class DayPlanAndWorkoutTimeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlans_WeekPlans_WeekPlanId",
                table: "DayPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTimes_DayPlans_DayPlanId",
                table: "WorkoutTimes");

            migrationBuilder.AlterColumn<int>(
                name: "DayPlanId",
                table: "WorkoutTimes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WeekPlanId",
                table: "DayPlans",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlans_WeekPlans_WeekPlanId",
                table: "DayPlans",
                column: "WeekPlanId",
                principalTable: "WeekPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTimes_DayPlans_DayPlanId",
                table: "WorkoutTimes",
                column: "DayPlanId",
                principalTable: "DayPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlans_WeekPlans_WeekPlanId",
                table: "DayPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTimes_DayPlans_DayPlanId",
                table: "WorkoutTimes");

            migrationBuilder.AlterColumn<int>(
                name: "DayPlanId",
                table: "WorkoutTimes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "WeekPlanId",
                table: "DayPlans",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlans_WeekPlans_WeekPlanId",
                table: "DayPlans",
                column: "WeekPlanId",
                principalTable: "WeekPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTimes_DayPlans_DayPlanId",
                table: "WorkoutTimes",
                column: "DayPlanId",
                principalTable: "DayPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
