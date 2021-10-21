using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class reasonsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReasonsForClosings",
                table: "ReasonsForClosings");

            migrationBuilder.RenameTable(
                name: "ReasonsForClosings",
                newName: "ReasonsForClosing");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReasonsForClosing",
                table: "ReasonsForClosing",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReasonsForClosing",
                table: "ReasonsForClosing");

            migrationBuilder.RenameTable(
                name: "ReasonsForClosing",
                newName: "ReasonsForClosings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReasonsForClosings",
                table: "ReasonsForClosings",
                column: "ID");
        }
    }
}
