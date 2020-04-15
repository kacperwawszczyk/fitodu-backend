using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class AddedEmailAndPhoneNumberToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Clients");
        }
    }
}
