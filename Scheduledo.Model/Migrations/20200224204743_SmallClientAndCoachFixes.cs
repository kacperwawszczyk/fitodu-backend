using Microsoft.EntityFrameworkCore.Migrations;

namespace Scheduledo.Model.Migrations
{
    public partial class SmallClientAndCoachFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwaitingTrainings_Clients_ClientId",
                table: "AwaitingTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_AwaitingTrainings_Coaches_CoachId",
                table: "AwaitingTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachClients_Clients_ClientId",
                table: "CoachClients");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachClients_Coaches_CoachId",
                table: "CoachClients");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Coaches_CoachId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Maximums_Clients_ClientId",
                table: "Maximums");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedTrainings_Coaches_CoachId",
                table: "PlannedTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Clients_ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Coaches_CoachId",
                table: "PrivateNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotes_Clients_ClientId",
                table: "PublicNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicNotes_Coaches_CoachId",
                table: "PublicNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Clients_ClientId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Coaches_CoachId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_Coaches_CoachId",
                table: "WorkTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_CoachId",
                table: "WorkTimes");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_ClientId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_CoachId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_PublicNotes_ClientId",
                table: "PublicNotes");

            migrationBuilder.DropIndex(
                name: "IX_PublicNotes_CoachId",
                table: "PublicNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_CoachId",
                table: "PrivateNotes");

            migrationBuilder.DropIndex(
                name: "IX_PlannedTrainings_CoachId",
                table: "PlannedTrainings");

            migrationBuilder.DropIndex(
                name: "IX_Maximums_ClientId",
                table: "Maximums");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_CoachId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_CoachClients_ClientId",
                table: "CoachClients");

            migrationBuilder.DropIndex(
                name: "IX_CoachClients_CoachId",
                table: "CoachClients");

            migrationBuilder.DropIndex(
                name: "IX_AwaitingTrainings_ClientId",
                table: "AwaitingTrainings");

            migrationBuilder.DropIndex(
                name: "IX_AwaitingTrainings_CoachId",
                table: "AwaitingTrainings");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PublicNotes");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "PublicNotes");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "PrivateNotes");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "PlannedTrainings");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Maximums");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "CoachClients");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "CoachClients");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AwaitingTrainings");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "AwaitingTrainings");

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "Clients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "WorkTimes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "PublicNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "PublicNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "PrivateNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "PrivateNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "PlannedTrainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Maximums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "CoachClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "CoachClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AwaitingTrainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "AwaitingTrainings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_CoachId",
                table: "WorkTimes",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_ClientId",
                table: "Trainings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_CoachId",
                table: "Trainings",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotes_ClientId",
                table: "PublicNotes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotes_CoachId",
                table: "PublicNotes",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_ClientId",
                table: "PrivateNotes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_CoachId",
                table: "PrivateNotes",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedTrainings_CoachId",
                table: "PlannedTrainings",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Maximums_ClientId",
                table: "Maximums",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CoachId",
                table: "Exercises",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_ClientId",
                table: "CoachClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_CoachId",
                table: "CoachClients",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_ClientId",
                table: "AwaitingTrainings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_CoachId",
                table: "AwaitingTrainings",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_AwaitingTrainings_Clients_ClientId",
                table: "AwaitingTrainings",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AwaitingTrainings_Coaches_CoachId",
                table: "AwaitingTrainings",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachClients_Clients_ClientId",
                table: "CoachClients",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachClients_Coaches_CoachId",
                table: "CoachClients",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Coaches_CoachId",
                table: "Exercises",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maximums_Clients_ClientId",
                table: "Maximums",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedTrainings_Coaches_CoachId",
                table: "PlannedTrainings",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateNotes_Clients_ClientId",
                table: "PrivateNotes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateNotes_Coaches_CoachId",
                table: "PrivateNotes",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotes_Clients_ClientId",
                table: "PublicNotes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicNotes_Coaches_CoachId",
                table: "PublicNotes",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Clients_ClientId",
                table: "Trainings",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Coaches_CoachId",
                table: "Trainings",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_Coaches_CoachId",
                table: "WorkTimes",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
