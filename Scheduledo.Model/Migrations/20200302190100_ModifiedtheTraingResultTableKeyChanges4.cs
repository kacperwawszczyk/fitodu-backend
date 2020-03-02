using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scheduledo.Model.Migrations
{
    public partial class ModifiedtheTraingResultTableKeyChanges4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "IdTrainingResult",
                table: "TrainingResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TrainingResults_IdTrainingResult",
                table: "TrainingResults",
                column: "IdTrainingResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults",
                column: "IdTrainingResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrainingResults",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingResults",
                table: "TrainingResults",
                column: "Id");
        }
    }
}
