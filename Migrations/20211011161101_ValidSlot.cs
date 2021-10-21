using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class ValidSlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ValidSlot",
                table: "ParkingSlots",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidSlot",
                table: "ParkingSlots");
        }
    }
}
