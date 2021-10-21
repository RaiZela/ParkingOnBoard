using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class CityReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ReasonID",
                table: "Cities",
                column: "ReasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ReasonForClosing_ReasonID",
                table: "Cities",
                column: "ReasonID",
                principalTable: "ReasonForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ReasonForClosing_ReasonID",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ReasonID",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "Cities");
        }
    }
}
