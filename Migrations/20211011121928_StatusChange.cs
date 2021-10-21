using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class StatusChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Users",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Streets",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "States",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ParkingSlots",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Cities",
                newName: "IsAvailable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Streets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "States",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "ParkingSlots",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Cities",
                newName: "Status");
        }
    }
}
