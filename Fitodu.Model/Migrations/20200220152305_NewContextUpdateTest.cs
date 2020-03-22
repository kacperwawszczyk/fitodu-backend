using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class NewContextUpdateTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "PrivateNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "PrivateNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "PrivateNotes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Surname = table.Column<string>(maxLength: 30, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(3, 2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(3, 2)", nullable: true),
                    FatPercentage = table.Column<decimal>(type: "decimal(2, 2)", nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressPostalCode = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    AddressState = table.Column<string>(nullable: true),
                    AddressCountry = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Surname = table.Column<string>(maxLength: 30, nullable: false),
                    Rules = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressPostalCode = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    AddressState = table.Column<string>(nullable: true),
                    AddressCountry = table.Column<string>(nullable: true),
                    VatIn = table.Column<string>(nullable: true),
                    TimeToResign = table.Column<string>(nullable: true),
                    EndOfLicenseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdTraining = table.Column<int>(nullable: false),
                    WeightChange = table.Column<decimal>(type: "decimal(3, 2)", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingExercises",
                columns: table => new
                {
                    IdExercise = table.Column<int>(nullable: false),
                    IdTraining = table.Column<int>(nullable: false),
                    Repeats = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TrainerNote = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingExercises", x => new { x.IdTraining, x.IdExercise });
                });

            migrationBuilder.CreateTable(
                name: "TrainingResults",
                columns: table => new
                {
                    IdExercise = table.Column<int>(nullable: false),
                    IdTraining = table.Column<int>(nullable: false),
                    Repeats = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingResults", x => new { x.IdExercise, x.IdTraining });
                });

            migrationBuilder.CreateTable(
                name: "Maximums",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdClient = table.Column<string>(nullable: true),
                    IdExercise = table.Column<string>(nullable: true),
                    Max = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maximums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maximums_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AwaitingTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCoach = table.Column<string>(nullable: false),
                    IdClient = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwaitingTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwaitingTrainings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AwaitingTrainings_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoachClients",
                columns: table => new
                {
                    IdClient = table.Column<string>(nullable: false),
                    IdCoach = table.Column<string>(nullable: false),
                    Place = table.Column<string>(type: "text", nullable: true),
                    PurchasedTrainings = table.Column<int>(nullable: false),
                    AvailableTrainings = table.Column<int>(nullable: false),
                    TimeToResign = table.Column<string>(maxLength: 30, nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachClients", x => new { x.IdCoach, x.IdClient });
                    table.ForeignKey(
                        name: "FK_CoachClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoachClients_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCoach = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlannedTrainings",
                columns: table => new
                {
                    IdCoach = table.Column<string>(nullable: false),
                    IdTraining = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedTrainings", x => new { x.IdCoach, x.IdTraining });
                    table.ForeignKey(
                        name: "FK_PlannedTrainings_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotes",
                columns: table => new
                {
                    IdCoach = table.Column<string>(nullable: false),
                    IdClient = table.Column<string>(nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotes", x => new { x.IdCoach, x.IdClient });
                    table.ForeignKey(
                        name: "FK_PublicNotes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicNotes_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdClient = table.Column<string>(nullable: true),
                    IdCoach = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trainings_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCoach = table.Column<string>(nullable: false),
                    Day = table.Column<DateTime>(type: "date", nullable: true),
                    StartHour = table.Column<TimeSpan>(nullable: true),
                    EndHour = table.Column<TimeSpan>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTimes_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_ClientId",
                table: "PrivateNotes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_CoachId",
                table: "PrivateNotes",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_ClientId",
                table: "AwaitingTrainings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AwaitingTrainings_CoachId",
                table: "AwaitingTrainings",
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
                name: "IX_Exercises_CoachId",
                table: "Exercises",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Maximums_ClientId",
                table: "Maximums",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedTrainings_CoachId",
                table: "PlannedTrainings",
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
                name: "IX_Trainings_ClientId",
                table: "Trainings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_CoachId",
                table: "Trainings",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_CoachId",
                table: "WorkTimes",
                column: "CoachId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Clients_ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateNotes_Coaches_CoachId",
                table: "PrivateNotes");

            migrationBuilder.DropTable(
                name: "AwaitingTrainings");

            migrationBuilder.DropTable(
                name: "CoachClients");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Maximums");

            migrationBuilder.DropTable(
                name: "PlannedTrainings");

            migrationBuilder.DropTable(
                name: "PublicNotes");

            migrationBuilder.DropTable(
                name: "Summaries");

            migrationBuilder.DropTable(
                name: "TrainingExercises");

            migrationBuilder.DropTable(
                name: "TrainingResults");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "WorkTimes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_CoachId",
                table: "PrivateNotes");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PrivateNotes");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "PrivateNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "PrivateNotes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
