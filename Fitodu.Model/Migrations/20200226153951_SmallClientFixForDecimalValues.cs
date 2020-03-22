using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class SmallClientFixForDecimalValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Clients",
                type: "decimal(5, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Clients",
                type: "decimal(5, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FatPercentage",
                table: "Clients",
                type: "decimal(4, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2, 2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Clients",
                type: "decimal(3, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Clients",
                type: "decimal(3, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FatPercentage",
                table: "Clients",
                type: "decimal(2, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 2)",
                oldNullable: true);
        }
    }
}
