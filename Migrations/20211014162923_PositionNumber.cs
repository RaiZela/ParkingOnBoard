using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class PositionNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionNumber",
                table: "ParkingSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionNumber",
                table: "ParkingSlots");
        }
    }
}
