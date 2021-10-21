using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class reasonPropstreet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_ReasonsForClosing_ReasonID",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_States_ReasonsForClosing_ReasonID",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_ReasonID",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ReasonID",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "States");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "Cities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "States",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_ReasonID",
                table: "States",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ReasonID",
                table: "Cities",
                column: "ReasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_ReasonsForClosing_ReasonID",
                table: "Cities",
                column: "ReasonID",
                principalTable: "ReasonsForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_States_ReasonsForClosing_ReasonID",
                table: "States",
                column: "ReasonID",
                principalTable: "ReasonsForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
