using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class AddedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "WorkTimes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "WeekPlans",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "AwaitingTrainings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "AwaitingTrainings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_IdCoach",
                table: "WorkTimes",
                column: "IdCoach");

            migrationBuilder.CreateIndex(
                name: "IX_WeekPlans_IdCoach",
                table: "WeekPlans",
                column: "IdCoach");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_IdClient",
                table: "Trainings",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_IdCoach",
                table: "Trainings",
                column: "IdCoach");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingExercises_IdExercise",
                table: "TrainingExercises",
                column: "IdExercise");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingExercises_IdTraining",
                table: "TrainingExercises",
                column: "IdTraining");

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_IdTraining",
                table: "Summaries",
                column: "IdTraining");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Maximums_IdClient",
                table: "Maximums",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IdCoach",
                table: "Exercises",
                column: "IdCoach");

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_IdClient",
                table: "CoachClients",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_IdClient",
                table: "AwaitingTrainings",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_IdCoach",
                table: "AwaitingTrainings",
                column: "IdCoach");

            migrationBuilder.AddForeignKey(
                name: "FK_AwaitingTrainings_Clients_IdClient",
                table: "AwaitingTrainings",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AwaitingTrainings_Coaches_IdCoach",
                table: "AwaitingTrainings",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachClients_Clients_IdClient",
                table: "CoachClients",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachClients_Coaches_IdCoach",
                table: "CoachClients",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Coaches_IdCoach",
                table: "Exercises",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maximums_Clients_IdClient",
                table: "Maximums",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maximums_Exercises_IdExercise",
                table: "Maximums",
                column: "IdExercise",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateNotes_Clients_IdClient",
                table: "PrivateNotes",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateNotes_Coaches_IdCoach",
                table: "PrivateNotes",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotes_Clients_IdClient",
                table: "PublicNotes",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotes_Coaches_IdCoach",
                table: "PublicNotes",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Trainings_IdTraining",
                table: "Summaries",
                column: "IdTraining",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingExercises_Exercises_IdExercise",
                table: "TrainingExercises",
                column: "IdExercise",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingExercises_Trainings_IdTraining",
                table: "TrainingExercises",
                column: "IdTraining",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Coaches_IdCoach",
                table: "Trainings",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeekPlans_Coaches_IdCoach",
                table: "WeekPlans",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_Coaches_IdCoach",
                table: "WorkTimes",
                column: "IdCoach",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwaitingTrainings_Clients_IdClient",
                table: "AwaitingTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_AwaitingTrainings_Coaches_IdCoach",
                table: "AwaitingTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachClients_Clients_IdClient",
                table: "CoachClients");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachClients_Coaches_IdCoach",
                table: "CoachClients");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Coaches_IdCoach",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Maximums_Clients_IdClient",
                table: "Maximums");

            migrationBuilder.DropForeignKey(
                name: "FK_Maximums_Exercises_IdExercise",
                table: "Maximums");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Clients_IdClient",
                table: "PrivateNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Coaches_IdCoach",
                table: "PrivateNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotes_Clients_IdClient",
                table: "PublicNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotes_Coaches_IdCoach",
                table: "PublicNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Trainings_IdTraining",
                table: "Summaries");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingExercises_Exercises_IdExercise",
                table: "TrainingExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingExercises_Trainings_IdTraining",
                table: "TrainingExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Clients_IdClient",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Coaches_IdCoach",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_WeekPlans_Coaches_IdCoach",
                table: "WeekPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_Coaches_IdCoach",
                table: "WorkTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_IdCoach",
                table: "WorkTimes");

            migrationBuilder.DropIndex(
                name: "IX_WeekPlans_IdCoach",
                table: "WeekPlans");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_IdClient",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_IdCoach",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_TrainingExercises_IdExercise",
                table: "TrainingExercises");

            migrationBuilder.DropIndex(
                name: "IX_TrainingExercises_IdTraining",
                table: "TrainingExercises");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_IdTraining",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes");

            migrationBuilder.DropIndex(
                name: "IX_Maximums_IdClient",
                table: "Maximums");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_IdCoach",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_CoachClients_IdClient",
                table: "CoachClients");

            migrationBuilder.DropIndex(
                name: "IX_AwaitingTrainings_IdClient",
                table: "AwaitingTrainings");

            migrationBuilder.DropIndex(
                name: "IX_AwaitingTrainings_IdCoach",
                table: "AwaitingTrainings");

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "WorkTimes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "WeekPlans",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCoach",
                table: "AwaitingTrainings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "IdClient",
                table: "AwaitingTrainings",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
