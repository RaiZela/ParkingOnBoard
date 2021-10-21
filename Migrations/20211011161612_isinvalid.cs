using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class isinvalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsValid",
                table: "ParkingSlots",
                newName: "IsInValid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInValid",
                table: "ParkingSlots",
                newName: "IsValid");
        }
    }
}
