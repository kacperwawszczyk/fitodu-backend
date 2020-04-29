using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitodu.Model.Migrations
{
    public partial class ClientAndNotesRelationshipChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes",
                column: "IdClient",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes",
                column: "IdClient",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes");

            migrationBuilder.DropIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotes_IdClient",
                table: "PublicNotes",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateNotes_IdClient",
                table: "PrivateNotes",
                column: "IdClient");
        }
    }
}
